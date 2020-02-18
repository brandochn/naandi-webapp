using System.Collections.Generic;
using Naandi.Shared.Models;

namespace WebApp.Areas.SocialWork.Models
{
    public class IngresosMensualesViewModel : IngresosEgresosMensualesMovimientoRelation
    {
        private string key;

        public string Key { get => key; set => key = value; }
        public IList<Movimiento> MovimientoList { get; set; }        
    }
}