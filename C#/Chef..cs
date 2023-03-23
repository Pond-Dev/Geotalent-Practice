namespace Test 
{
    class Chef 
    {
        public void MakeChicken ()
        {
            System.Console.WriteLine("The Chef makes chicken");

        }
        public void MakeSalad ()
        {
            System.Console.WriteLine("The Chef makes salad");

        }

        public virtual void makeSpecialDish(){
            System.Console.WriteLine("The Chef makes bbq rips");
        }
    }
}