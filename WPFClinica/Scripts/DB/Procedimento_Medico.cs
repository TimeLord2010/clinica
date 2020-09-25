using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFClinica.Scripts.DB {

    class Procedimento_Medico : Template {

        static Procedimento_Medico pm = new Procedimento_Medico();

        public override string TableName {
            get => nameof(Procedimento_Medico);
        }

        public override Action GetCT() {
            return () => {
                SQLOperations.NonQuery("Erro ao criar procedimento-medico",
                    $"create table if not exists {TableName} (" +
                    $"  historico int," +
                    $"  medico varchar(15)," +
                    $"  primary key (historico)," +
                    $"  foreign key (historico) references {Historico_Procedimento.Name} ({nameof(Historico_Procedimento.ID)}) on delete cascade," +
                    $"  foreign key (medico) references {Medicos.Name} ({nameof(Medicos.CPF)}) on delete cascade on update cascade" +
                    $");");
            };
        }

        public static void Insert(int historico, string cpf) {
            using (var c = new MySqlCommand($"insert into {pm.TableName} values ({historico}, @cpf);")) {
                c.Parameters.AddWithValue("@cpf", cpf);
                pm.NonQuery($"Erro ao inserir em procedimento-medico. ({ErrorCodes.DB0056})", c);
            }
        }

        public static (string cpfMedico, string nomeMedico) Select(int historico) {
            var res = (cpfMedico: "", nomeMedico: "");
            using (var c = new MySqlCommand(
                $"select " +
                $"  a.medico," +
                $"  b.{nameof(Pessoas.Nome)} " +
                $"from {pm.TableName} as a " +
                $"left join {Pessoas.p.TableName} as b on b.{nameof(Pessoas.CPF)} = a.medico " +
                $"where historico = {historico};")) {
                pm.QueryRLoop($"Erro ao obter em procedimento-medico. ({ErrorCodes.DB0057})", c, (r) => {
                    res.cpfMedico = r.GetString(0);
                    res.nomeMedico = r.IsDBNull(1) ? null : r.GetString(1);
                });
                return res;
            }
        }

    }
}
