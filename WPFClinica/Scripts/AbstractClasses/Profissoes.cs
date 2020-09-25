using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFClinica.Scripts.DB;

namespace WPFClinica.Scripts.AbstractClasses {

    public abstract class Profissoes : Template {

        public override Action GetCT() {
            return () => {
                SQLOperations.NonQuery($"Erro ao criar profissao: {TableName}",
                    $"create table if not exists {TableName} (" +
                    $"  cpf varchar(15)," +
                    $"  primary key (cpf)," +
                    $"  foreign key (cpf) references funcionarios(cpf) ON DELETE CASCADE ON UPDATE CASCADE" +
                    $");");
            };
        }

        public void Insert(string cpf) {
            using (var c = new MySqlCommand($"insert into {TableName} values (@cpf);")) {
                c.Parameters.AddWithValue("@cpf", cpf);
                NonQuery($"Erro ao inserir {TableName}.", c);
            }
        }

        public void TryInsert (string cpf) {
            if (!IsProfissional(cpf)) Insert(cpf);
        }

        public void Delete(string cpf) {
            using (var c = new MySqlCommand($"delete from {TableName} where cpf = @cpf;")) {
                c.Parameters.AddWithValue("@cpf", cpf);
                NonQuery($"Erro ao deletar {TableName}.", c);
            }
        }

        public bool IsProfissional(string cpf) {
            using (var c = new MySqlCommand($"select * from {TableName} where cpf = @cpf;")) {
                c.Parameters.AddWithValue("@cpf", cpf);
                bool a = false;
                QueryRLoop("Erro ao obter administrador", c, (r) => {
                    a = true;
                });
                return a;
            }
        }

        public void Update (bool isProfissional, string cpf) {
            if (isProfissional) {
                TryInsert(cpf);
            } else {
                Delete(cpf);
            }
        }

    }

}
