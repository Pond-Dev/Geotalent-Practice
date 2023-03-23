namespace Test 
{
    class ItalianChef : Chef
    {
        public void MakePasta () 
        {
            System.Console.WriteLine("The Chef makes pasta");
        }
        public override void makeSpecialDish(){
            System.Console.WriteLine("The Chef makes chicken parm");
        }
    }
}