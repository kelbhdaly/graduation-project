namespace MedicalApp.BusinessLogic.DTOs.AiDtos
{
    public class LungRiskHistoryDto
    {
        public DateTime CreatedAt { get; set; }

        public int Obesity { get; set; }
        public int CoughingOfBlood { get; set; }
        public int AlcoholUse { get; set; }
        public int DustAllergy { get; set; }
        public int PassiveSmoker { get; set; }
        public int BalancedDiet { get; set; }
        public int GeneticRisk { get; set; }
        public int OccupationalHazards { get; set; }
        public int ChestPain { get; set; }
        public int AirPollution { get; set; }

        public string Result { get; set; }

        public double Low { get; set; }
        public double Medium { get; set; }
        public double High { get; set; }
    }
}
