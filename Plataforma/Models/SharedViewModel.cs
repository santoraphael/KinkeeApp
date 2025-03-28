namespace Plataforma.Models
{
    public class SharedViewModel
    {
        public LoginViewModel LoginViewModel { get; set; }
        public PerfilViewModel PerfilViewModel { get; set; }
        public CadastroViewModel CadastroViewModel { get; set; }
        public ForgotPasswordViewModel ForgotPasswordViewModel { get; set; }
        public MessageViewModel MessageViewModel { get; set; }
        public NotificationViewModel NotificationViewModel { get; set; }

        public bool booleanVariable { get; set; }

    }
}