using System.Collections.Generic;
using System.Linq;
using Naandi.Shared.Models;
using Naandi.Shared.Services;

namespace WebApp.Areas.SocialWork.Models
{
    public class IngresosMensualesViewModel : IngresosEgresosMensualesMovimientoRelation
    {
        private string key;

        public string Key { get => key; set => key = value; }
        public IList<Movimiento> MovimientoList { get; set; }

        public void LoadMovimientoList(IFamilyResearch familyResearchRepository)
        {
            MovimientoList = familyResearchRepository.GetMovimientosByTipoMovimiento("Ingreso").ToList();
            MovimientoList.Insert(0, new Movimiento()
            {
                Name = "Selecciona uno"
            });
        }
    }
}