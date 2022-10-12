namespace WPFSTD105
{
    internal class BarcodeGenerator
    {
        private object code128;
        private string v;

        public BarcodeGenerator(object code128, string v)
        {
            this.code128 = code128;
            this.v = v;
        }
    }
}