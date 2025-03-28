namespace Plataforma.Models
{
    public class SearchModel
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public int Tipo { get; set; }
    }

    public enum SearchTipo
    {
        Usuario = 1,
        Cidade = 2,
        Estado = 3
    };
}