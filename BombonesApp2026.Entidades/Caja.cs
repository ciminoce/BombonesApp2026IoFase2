namespace BombonesApp2026.Entidades
{
    public class Caja : Producto
    {
        private List<DetalleCaja> _detalles = new();
        public int CantidadBombones => _detalles.Sum(d => d.Cantidad);
        public bool EsSurtida => _detalles.Count() > 1;
        public decimal Precio => Math
            .Ceiling(_detalles.Sum(d => d.Subtotal) * 1.2m / 100) * 100;
        public IReadOnlyCollection<DetalleCaja> Detalles => _detalles;
        public Caja()
        {
            
        }
        public Caja(
            string nombre,
            int stock,
            string? descripcion) : base(nombre,  stock, descripcion)
        {

        }

        public override string MostrarDatos()
        {
            string tipoCaja = EsSurtida ? "Surtida" : "Especial";
            return $"Caja ({tipoCaja}): {Nombre} - Precio: {Precio:C2} - Cant. Bombones: {CantidadBombones} unidades";
        }
    }
}
