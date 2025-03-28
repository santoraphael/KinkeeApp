using Mongo.Conn;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Mongo.INFRA
{
    public class ImagemHelper
    {
        private readonly Connection db = new Connection();

        // Apenas um placeholder, não necessariamente precisa ser o Main de fato.
        public void Main(string[] args)
        {
            // Exemplo de uso (desativado):
            // var id = UploadFile("D:\\foto.png");
            // DownloadFile(id, "D:\\baixado.png");
        }

        /// <summary>
        /// Exemplo de método que faz o upload de um arquivo com a nova API GridFS.
        /// </summary>
        public ObjectId UploadFile(string caminho)
        {
            var database = db.ConnectServer();   // Retorna IMongoDatabase
            var gridFs = new GridFSBucket(database);

            using (var stream = File.OpenRead(caminho))
            {
                // UploadFromStreamAsync retorna o Task<ObjectId>
                var task = gridFs.UploadFromStreamAsync(Path.GetFileName(caminho), stream);
                task.Wait(); // Bloqueia para aguardar o ID gerado
                return task.Result;
            }
        }

        /// <summary>
        /// Exemplo de método que baixa um arquivo do GridFS para o disco usando a nova API GridFS.
        /// </summary>
        public void DownloadFile(ObjectId fileId, string destino)
        {
            var database = db.ConnectServer();
            var gridFs = new GridFSBucket(database);

            using (var fileStream = File.Create(destino))
            {
                // Baixa o arquivo usando o ID
                var task = gridFs.DownloadToStreamAsync(fileId, fileStream);
                task.Wait();
            }
        }

        /// <summary>
        /// Mantendo uma lógica semelhante ao método 'Subir' original,
        /// mas agora usando a API nova GridFSBucket.
        /// </summary>
        public void Subir(string caminho)
        {
            var database = db.ConnectServer();
            var gridFs = new GridFSBucket(database);

            // Nome do arquivo para teste
            var newFileName = "D:\\new_Untitled.png";

            // Faz upload
            ObjectId fileId;
            using (var fs = new FileStream(caminho, FileMode.Open))
            {
                var uploadTask = gridFs.UploadFromStreamAsync(Path.GetFileName(caminho), fs);
                uploadTask.Wait();
                fileId = uploadTask.Result;
            }

            // Faz download (usando o ID retornado)
            using (var memory = new MemoryStream())
            {
                var downloadTask = gridFs.DownloadToStreamAsync(fileId, memory);
                downloadTask.Wait();

                // Salva o conteúdo baixado em um novo arquivo no disco
                memory.Position = 0;
                using (var newFs = new FileStream(newFileName, FileMode.Create))
                {
                    memory.CopyTo(newFs);
                }
            }
        }
    }
}
