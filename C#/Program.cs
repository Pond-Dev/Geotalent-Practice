using System;

namespace Test
{
 class Program 
 {
    static void Main(string[] args)
    {
            //  constructor

    // Book book1=  new Book("Harry Porter","JK Rowling",400);
    // Book book2=  new Book("Lord of the rings","Tolkein",700);
    // System.Console.WriteLine(book1.author);

            // object methods

    // Student student1 = new Student("Jim","Business",2.8);
    // Student student2 = new Student("Pam","Art",3.6);
    // System.Console.WriteLine(student1.HasHonor());
    // System.Console.WriteLine(student2.HasHonor());

            // get set

    // Movie avenger = new Movie("The Avengers","Joss Whedon","Dog");
    // Movie shrek = new Movie("Shrek","Adam Adamson","PG");
    // System.Console.WriteLine(avenger.Rating);

            // static class

    // Song holiday = new Song("Holiday","Green Day",200);
    // System.Console.WriteLine(Song.songCount);
    // Song Kashmir = new Song ("Kashmir","Led Zeppelin",1000);
    // System.Console.WriteLine(Kashmir.getSongCount());

            // static method

    // UsefulTools.SayHi("test");
    
            // inheritance

   Chef chef = new Chef();
   ItalianChef ItalianChef = new ItalianChef();
    // chef.MakeChicken();
    // ItalianChef.MakeSalad();
    chef.makeSpecialDish();
    ItalianChef.makeSpecialDish();
    
    }
 }

 
}
