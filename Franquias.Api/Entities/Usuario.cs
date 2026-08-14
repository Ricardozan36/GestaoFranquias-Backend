namespace Franquias.Api.Entities
{
    public enum Perfil { Administrador, GestorUnidade, Operador }
    public enum StatusUnidade { Ativa, Inativa }
    public enum StatusChamado { Aberto, EmAtendimento, Fechado }

    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;
        public Perfil Perfil { get; set; }
        public bool Ativo { get; set; } = true;
    }
}