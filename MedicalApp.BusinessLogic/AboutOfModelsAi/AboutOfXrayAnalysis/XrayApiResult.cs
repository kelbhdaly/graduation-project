namespace MedicalApp.BusinessLogic.AboutOfModelsAi.AboutOfXrayAnalysis
{
    public  class XrayApiResult
    {
        public string Filename { get; set; }
        public string Predicted_Class { get; set; }
        public double Confidence { get; set; }
        public Dictionary<string, double> Class_Probabilities { get; set; }
    }
}
