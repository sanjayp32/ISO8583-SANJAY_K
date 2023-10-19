using ISO;
using sanjay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Programs
{
    static void Main(string[] args)
    {
        Console.WriteLine("Enter Message Option: \n1 -> SignOn\n2 -> SignOff\n3 -> BalanceInquiry\n4 -> CashWithdrawal\n5 -> EmvTags\n7 -> Exit -> To Exit The Program");

        int msgRequired = 0;

        while (true)
        {
            string option = Console.ReadLine();
            option= option.ToLower();
            if (option != "exit")
            {
                if (int.TryParse(option, out msgRequired))
                {
                    Console.WriteLine(MessageFactory.ConstructMessage((RequiredMsg)msgRequired));
                }
                else
                {
                    Console.WriteLine("Invalid option. Please enter an integer.");
                }
            }
            else
            {
                Environment.Exit(0);
            }

        }
    }
}
