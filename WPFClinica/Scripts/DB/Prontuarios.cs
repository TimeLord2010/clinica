using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;
using static WPFClinica.Scripts.DB.SQLOperations;

namespace WPFClinica.Scripts.DB {

    public class Prontuarios : Template {

        static readonly Prontuarios p = new Prontuarios();

        public string Paciente { get; set; }
        public byte[] FileData { get; set; }
        public int FileSize { get; set; }

        public override string TableName => "prontuarios";

        public static void Insert(string paciente, FileInfo fileInfo) {
            var c = new MySqlCommand($"insert into {p.TableName} values (@paciente, @fileData, @fileSize);");
            c.Parameters.AddWithValue("@paciente", paciente);
            c.Parameters.AddWithValue("@fileData", File.ReadAllBytes(fileInfo.FullName));
            c.Parameters.AddWithValue("@fileSize", fileInfo.Length);
            p.NonQuery("Erro ao inserir prontuário.", c);
        }

        /// <summary>
        /// Paciente, FileName, FileData, FileSize
        /// </summary>
        /// <param name="paciente"></param>
        public static void Select(string paciente, out byte[] fileData) {
            var c = new MySqlCommand($"select {nameof(FileData)}, {nameof(FileSize)} " +
            $"from {p.TableName} " +
            $"where {nameof(Paciente)} = @paciente;");
            c.Parameters.AddWithValue("@paciente", paciente);
            byte[] fd = null;
            p.QueryRLoop("Erro ao obter prontuário.", c, (r) => {
                var len = r.GetInt32(1);
                fd = new byte[len];
                r.GetBytes(0, 0, fd, 0, len);
            });
            fileData = fd;
        }

        public static void SelectOrCreate(string paciente, out byte[] fileData) {
            fileData = null;
            for (int i = 0; i < 2; i++) {
                try {
                    Select(paciente, out fileData);
                    if (fileData == null) throw new Exception();
                    return;
                } catch (Exception) {
                    var fn = "Prontuário.docx";
                    using (var s = new FileStream(fn, FileMode.Create, FileAccess.Write)) {
                        var d = Properties.Resources.Modelo_Prontuário;
                        s.Write(d, 0, d.Length);
                    }
                    var fi = new FileInfo(fn);
                    Insert(paciente, fi);
                    File.Delete(fn);
                }
            }
        }

        public static void Update(string paciente, FileInfo fileInfo) {
            var c = new MySqlCommand($"update {p.TableName} set " +
                $"  {nameof(FileData)} = @fileData, " +
                $"  {nameof(FileSize)} = @fileSize " +
                $"where " +
                $"  {nameof(Paciente)} = @paciente;");
            c.Parameters.AddWithValue("@paciente", paciente);
            c.Parameters.AddWithValue("@fileData", File.ReadAllBytes(fileInfo.FullName));
            c.Parameters.AddWithValue("@fileSize", fileInfo.Length);
            p.NonQuery("Erro ao atualizar o prontuário.", c);
        }

        public override Action GetCT() {
            return () => {
                var c = new MySqlCommand($"create table if not exists {p.TableName} (" +
                    $"  {nameof(Paciente)} varchar(15)," +
                    $"  {nameof(FileData)} longblob not null," +
                    $"  {nameof(FileSize)} int not null," +
                    $"  primary key ({nameof(Paciente)})," +
                    $"  foreign key ({nameof(Paciente)}) references {Pacientes.Name} ({nameof(Pessoas.CPF)}) on delete cascade on update cascade" +
                    $");");
                p.NonQuery("Erro ao criar tabela de prontuários.", c);
            };
        }
    }

}
