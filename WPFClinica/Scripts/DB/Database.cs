using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Reflection;

namespace WPFClinica.Scripts.DB {

    public static class Database {

        //public const string Name = "clinica";

        /*public static void Drop() {
            SQLOperations.NonQuery("Erro ao deletar banco de dados", $"drop database {Name};");
        }*/

        private static Type[] GetTypesInNamespace(Assembly assembly, string nameSpace) {
            return assembly.GetTypes().Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal)).ToArray();
        }

        public static void EnsureCreation() {
            //try {
                SQLOperations.MessageExceptions = true;
                SQLOperations.ThrowExceptions = true;
                //SQLOperations.NonQuery("Erro ao criar banco de dados.", $"create database if not exists {Name};");
                Pessoas.p.CreateTable();
                Pacientes.CreateTable();
                Funcionarios.CreateTable();
                Medicos.CreateTable();
                Recepcionista.CreateTable();
                Tecnico_Enfermagem.CreateTable();
                (new Administradores()).GetCT()();
                TempPacientes.CreateTable();
                ListaEspera.CreateTable();
                Salas.CreateTable();
                Paciente_Sala.CreateTable();
                Convenios.CreateTable();
                ProcedimentosLab.CreateTable();
                ProcedimentoConvenio.CreateTable();
                PacienteProcedimentos.CreateTable();
                Enderecos.CreateTable();
                Anexos.CreateTable();
                Especializacoes.CreateTable();
                MedicoEspecializacoes.CreateTable();
                Agendamentos.CreateTable();
                AgendamentoPaciente.CreateTable();
                AgendamentoTempPaciente.CreateTable();
                AgendamentoFuncionario.CreateTable();
                ListaEspera_Funcionario.CreateTable();
                ListaEspera_Especializacao.CreateTable();
                Historico_Consultas.CreateTable();
                Historico_ProcedimentosLab.CreateTable();
                Pagamentos.CreateTable();
                //Fonoaudiologos.CreateTable();
                Nutricionistas.CreateTable();
                Psicologos.CreateTable();
                Ausentes.CreateTable();
            /*} catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
            finally {*/
                SQLOperations.ThrowExceptions = false;
            //}
        }

        public static void Recreate() {
            //Drop();
            EnsureCreation();
        }

        public static TimeSpan GetTimeSpan(string input) {
            var parts = input.Split(':');
            var c = Array.ConvertAll(parts, x => Convert.ToInt32(x));
            return new TimeSpan(c[0], c[1], c[2]);
        }

    }
}
