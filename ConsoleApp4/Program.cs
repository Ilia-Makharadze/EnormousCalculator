using System;
using System.Linq;

public class CsharpProgram
{
    public static void Main(string[] args)
    {
        //Infint C = new Infint("9");
        //Infint D = new Infint("8");
        //Console.WriteLine(C.Minus(D));

        Console.WriteLine("//////////////////////////////////////////////////////////////////////////////");
        
        

        Console.WriteLine("Program calculator which calculate +,-,* operations ");
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("Enter the first number: ");
        string input1 = Console.ReadLine();

        Console.WriteLine("Enter the second number: ");
        string input2 = Console.ReadLine();


        Console.WriteLine("Enter the operation +, -, *");
        char operation = Console.ReadKey().KeyChar;
        Console.WriteLine();
        Infint A = new Infint(input1);
        Infint B = new Infint(input2);

       
        switch (operation)
        {
            case '+':
               
                Console.WriteLine($"{input1} + {input2} = {A.Plus(ref B)}");
                break;
            case '-':
                
                Console.WriteLine($"{input1} - {input2} = {A.Minus(B)}");
                break;
            case '*':
                
                Console.WriteLine($"{input1} * {input2} = {A.Times(B)}");
                break;
            
            default:
                Console.WriteLine("Invalid operation.");
                break;
                    
        }

        //Infint A = new Infint("1");
        //Infint B = new Infint("1000");
        //Console.WriteLine(A.Times(B));


        Console.ReadKey();
    }
}
class Infint
{
    private int[] numbers; 

    public Infint(int[] _numbers) { Numbers = _numbers; } // Constructor with an array of integers
    public int[] Numbers
    {
        get { return numbers; }
        set { numbers = value; }
    }

    public Infint(Infint other)// Copy constructor
    {
        numbers = other.numbers;
    }

    public Infint(String inputString)// Constructor from a string
    {
        bool neg = false;
        if (inputString[0] == '-')
        {
            neg = true;
        }
        numbers = new int[inputString.Length];

        if (neg)// Convert string to integer array, handling negative numbers
        {
            numbers[0] = -1;
            for (int i = 1; i < inputString.Length; i++)
            {
                numbers[i] = inputString[i] - '0';
            }
        }
        else
        {
            for (int i = 0; i < inputString.Length; i++)
            {
                numbers[i] = inputString[i] - '0';
            }
        }
    }

    public static int index(int[] arr, int n)// Helper method to get an element from an array with boundary check
    {
        if (n < 0 || n >= arr.Length) return 0;
        return arr[n];
    }

    public string Plus(ref Infint other)//Plus function
    {
      
        if (this.IsPositive() && !other.IsPositive())// Handle different sign combinations for addition
        {
            other.numbers = RemoveSign(other.numbers);
            this.Minus(other);
            return this.ToString();
        }
       
        else if (!this.IsPositive() && other.IsPositive())// If signs are different, convert to subtraction
        {
            Infint temp1 = new Infint(other);
            Infint temp2 = new Infint(this);
            temp2.numbers = RemoveSign(temp2.numbers);
            temp1.Minus(temp2);
            numbers = temp1.numbers;
            return this.ToString();
        }
        
        else if (!this.IsPositive() && !other.IsPositive())  // Otherwise, perform addition
        {
            
            other.numbers = RemoveSign(other.numbers);

            return this.Minus(other);
        }

        int maxLength = Math.Max(numbers.Length, other.numbers.Length);
        int temp = 0;
        int carry = 0;
        int[] workingArray = new int[maxLength + 1];

        for (int i = 0; i < maxLength; i++)
        {
            temp = carry + index(this.Numbers, this.Numbers.Length - 1 - i) + index(other.Numbers, other.Numbers.Length - 1 - i);
            carry = temp / 10;//remembred number
            temp = temp % 10;//in the array of 10 modulus variable
            workingArray[i] = temp;

        }
        if (carry != 0) workingArray[maxLength] = carry;//
        else Array.Resize(ref workingArray, maxLength);//

        Array.Reverse(workingArray);


        this.numbers = workingArray;



        return String.Join("", workingArray);  // Return the result as a string
    }


    public string Minus(Infint other) // Handle different sign combinations for subtraction
    {

        if (numbers[0] == 0 && other.Numbers[0] > 0)
        {
            Infint other2 = new Infint(other);
            numbers = InsertSign(other2.numbers);
            return new Infint(numbers).ToString();
        }

        bool equal = true;
        if (numbers.Length == other.numbers.Length && numbers[0] == other.numbers[0])
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] != other.numbers[i])
                {
                    equal = false;
                    break;
                }
            }
        }

        if (!equal)  // If signs are different, convert to addition
        {
            numbers = new int[] { 0 };

            return "0";
        }


        Infint other1 = new Infint(other);
        bool resultIsNegative = false;

        
        if (this.IsPositive() && !other.IsPositive())  // Otherwise, perform subtraction
        {
            other.numbers = RemoveSign(other.numbers);
            this.Plus(ref other);
            return this.ToString();
        }
       
        else if (!this.IsPositive() && other.IsPositive())
        {
            Infint temp = new Infint(this);
            temp.numbers = RemoveSign(temp.numbers);
            other.Plus(ref temp);
            numbers = other.numbers;
            numbers = InsertSign(numbers);
            return this.ToString();
        }
        
        else if (!this.IsPositive() && !other.IsPositive())
        {
            Infint temp = new Infint(this);
            if (this.IsSmaller(ref numbers, ref other.numbers))
            {
                other.numbers = RemoveSign(other.numbers);
                this.Plus(ref other);
                return this.ToString();
            }
            else
            {
                numbers = RemoveSign(numbers);
                other.numbers = RemoveSign(other.numbers);
                this.Minus(other);
                numbers = InsertSign(numbers);
                return this.ToString();
            }
        }

        if (this.IsSmaller(ref numbers, ref other1.numbers))
        {
            var temp = numbers;
            numbers = other1.numbers;
            other1.numbers = temp;
            resultIsNegative = true;
        }

        int size = Math.Max(numbers.Length, other1.numbers.Length);




        while (numbers.Length < size)
        {
            numbers = PrependZeros(numbers, numbers.Length + 1);
        }
        while (other1.numbers.Length < size)
        {
            other1.numbers = PrependZeros(other1.numbers, other1.numbers.Length + 1);
        }


        int borrow = 0;
        for (int i = size - 1; i >= 0; i--)
        {
            int diff = numbers[i] - other1.numbers[i] - borrow;
            if (diff < 0)
            {
                diff += 10;
                borrow = 1;
            }
            else
            {
                borrow = 0;
            }
            numbers[i] = diff;
        }


        int startIndex = Array.FindIndex(numbers, x => x != 0);
        if (startIndex == -1)
        {
            return "0";
        }
        else
        {
            Array.Copy(numbers, startIndex, numbers, 0, numbers.Length - startIndex);
            Array.Resize(ref numbers, numbers.Length - startIndex);
        }


        if (resultIsNegative)
        {
            numbers = InsertSign(numbers);
        }



        return (new Infint(numbers)).ToString();
    }

    public string Times(Infint mulOriginalNumber) 
    {
        Infint mulNumber = new Infint(mulOriginalNumber); 

        
        bool resultsNegative = false;

      
        if (this.IsPositive() && !mulNumber.IsPositive())
        {
            resultsNegative = true;
            mulNumber.numbers = RemoveSign(mulNumber.numbers);
        }
        
        else if (!this.IsPositive() && mulNumber.IsPositive())
        {
            numbers = RemoveSign(numbers);
            resultsNegative = true;
        }
        
        else if (!this.IsPositive() && !mulNumber.IsPositive())
        {
            numbers = RemoveSign(numbers);
            mulNumber.numbers = RemoveSign(mulNumber.numbers);
        }

        int[] result = new int[numbers.Length + mulNumber.numbers.Length]; 

        
        for (int i = numbers.Length - 1; i >= 0; i--)
        {
            for (int j = mulNumber.numbers.Length - 1; j >= 0; j--)
            {
                int mul = numbers[i] * mulNumber.numbers[j]; 
                int sum = mul + result[i + j + 1]; 
                result[i + j + 1] = sum % 10; 
                result[i + j] += sum / 10; 
            }
        }

        int startIndex = 0;
        while (result[startIndex] == 0 && startIndex < result.Length - 1) 
        {
            startIndex++;
        }

        if (resultsNegative) 
        {
            result = InsertSign(result);
        }

       
        numbers = result;


        char[] ans = new Infint(numbers).ToString().ToArray();

        if (resultsNegative)
            ans[0] = '-';

        return String.Join("", ans); 

       
    }


    private bool IsSmaller(ref int[] b1, ref int[] b2)
    {
        if (b1.Length == 1 && b1[0] == 0 && b2.Length == 1 && b2[0] == 0)
        {
            return false;
        }

        int[] num1 = new int[b1.Length];
        Array.Copy(b1, num1, b1.Length);
        int[] num2 = new int[b2.Length];
        Array.Copy(b2, num2, b2.Length);


        num1[0] = Math.Abs(num1[0]);
        num2[0] = Math.Abs(num2[0]);

        int size1 = num1.Length;
        int size2 = num2.Length;

        if (size1 < size2) return true;
        if (size2 < size1) return false;

        for (int i = 0; i < size1; i++)
        {
            if (num1[i] < num2[i]) return true;
            else if (num1[i] > num2[i]) return false;
        }

        return false;
    }

    int[] PrependZeros(int[] array, int size)
    {
        int[] result = new int[size];
        int offset = size - array.Length;
        Array.Copy(array, 0, result, offset, array.Length);
        return result;
    }

    public bool IsPositive()// Helper method to determine if the number is positive
    {
        return numbers[0] >= 0;
    }
    private int[] InsertSign(int[] number)// Helper method to insert a sign to the number array
    {
        int[] newNumber = new int[number.Length + 1];
        newNumber[0] = -1;
        Array.Copy(number, 0, newNumber, 1, number.Length);
        return newNumber;
    }
    private int[] RemoveSign(int[] number)// Helper method to remove the sign from the number array
    {
        if (number[0] == -1)
        {
            int[] newNumber = new int[number.Length - 1];
            Array.Copy(number, 1, newNumber, 0, newNumber.Length);
            return newNumber;
        }
        return number;
    }

    public override string ToString() // If the number is negative, remove the sign temporarily
                                      // Convert the number array to string and return
    {
        if (!this.IsPositive())
        {
            numbers = RemoveSign(numbers);
            numbers[0] *= -1;
        }

        return String.Join("", numbers);
    }

}




