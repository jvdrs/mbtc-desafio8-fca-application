using FcaApplication.Api.Domain.Enums;
using FcaApplication.Api.Domain.Repositories;
using System;

namespace FcaApplication.Api.Repository.GetConsumoByRatedCar
{
    public class GetDataByEntityRatedCarRepository : IRecommendRepository
    {
        private readonly string[] carList = new string[8];

        public string GetOrder(EntityType entityType, string ratedCar)
        {
            switch (entityType)
            {                
                case EntityType.SEGURANCA:
                    carList[0] = "FIAT 500";
                    carList[1] = "RENEGADE";
                    carList[2] = "CRONOS";
                    carList[3] = "LINEA";
                    carList[4] = "MAREA";
                    carList[5] = "ARGO";
                    carList[6] = "TORO";
                    carList[7] = "DUCATO";

                    break;
                case EntityType.CONSUMO:
                    carList[0] = "FIORINO";
                    carList[1] = "CRONOS";
                    carList[2] = "ARGO";
                    carList[3] = "LINEA";
                    carList[4] = "RENEGADE";
                    carList[5] = "MAREA";
                    carList[6] = "TORO";
                    carList[7] = "DUCATO";

                    break;
                case EntityType.DESEMPENHO:
                    carList[0] = "LINEA";
                    carList[1] = "MAREA";
                    carList[2] = "CRONOS";
                    carList[3] = "RENEGADE";
                    carList[4] = "ARGO";
                    carList[5] = "FIAT 500";
                    carList[6] = "TORO";
                    carList[7] = "DUCATO";

                    break;
                case EntityType.MANUTENCAO:
                    carList[0] = "FIORINO";
                    carList[1] = "LINEA";
                    carList[2] = "CRONOS";
                    carList[3] = "ARGO";
                    carList[4] = "MAREA";
                    carList[5] = "RENEGADE";
                    carList[6] = "TORO";
                    carList[7] = "DUCATO";

                    break;
                case EntityType.CONFORTO:
                    carList[0] = "LINEA";
                    carList[1] = "FIAT 500";
                    carList[2] = "CRONOS";
                    carList[3] = "RENEGADE";
                    carList[4] = "ARGO";
                    carList[5] = "MAREA";
                    carList[6] = "TORO";
                    carList[7] = "DUCATO";

                    break;
                case EntityType.DESIGN:
                    carList[0] = "FIAT 500";
                    carList[1] = "CRONOS";
                    carList[2] = "LINEA";
                    carList[3] = "ARGO";
                    carList[4] = "RENEGADE";
                    carList[5] = "FIORINO";
                    carList[6] = "TORO";
                    carList[7] = "DUCATO";

                    break;
                case EntityType.ACESSORIOS:
                    carList[0] = "LINEA";
                    carList[1] = "FIAT 500";
                    carList[2] = "RENEGADE";
                    carList[3] = "CRONOS";
                    carList[4] = "MAREA";
                    carList[5] = "ARGO";
                    carList[6] = "TORO";
                    carList[7] = "DUCATO";

                    break;
                default:
                    break;
            }

            var position = Array.FindIndex(carList, car => car.Equals(ratedCar, StringComparison.InvariantCultureIgnoreCase));           
            var suggestedCar = carList[0];

            // REGRA 2: O mesmo veículo passado no parâmetro de entrada não pode ser sugerido(ex: a API não pode recomendar o MAREA se o Marea for o carro trabalhado no review / comentário).
            if (suggestedCar.Equals(ratedCar, StringComparison.InvariantCultureIgnoreCase))
            {
                return carList[1];
            }

            return carList[0];
        }
        
    }
}
