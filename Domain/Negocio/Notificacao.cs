namespace Domain.Negocio
{
    public class Notificacao : EntidadeDominio
    {
        public int UsuarioId { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public byte Visualizada { get; set; }
    }
}
