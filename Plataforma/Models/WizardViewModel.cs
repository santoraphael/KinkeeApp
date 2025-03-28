namespace Plataforma.Models
{
    public class WizardViewModel
    {
        public bool CategoriaRelacionamentSugar { get; set; }
        public bool CategoriaPacksFotosFas { get; set; }
        public bool CategoriaBDSM { get; set; }

    }

    public enum Categorias
    {
        RelacionamentSugar = 1,
        PacksFotosFas = 2,
        BDSM = 3,
    }
}