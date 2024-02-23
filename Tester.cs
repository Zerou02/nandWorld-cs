public class Tester
{
    public static void test(IComponent component, bool[][] tests)
    {
        var result = new bool[tests.Length];
        var count = 0;
        foreach (var test in tests)
        {
            var inputs = component.getInputPins();
            var amountIns = inputs.Length;
            if (component.getType() == "nand")
            {
                amountIns = 2;
            }
            var k = 0;
            for (int i = 0; i < amountIns; i++)
            {
                inputs[i].state = test[i];
                k = i;
            }
            component.eval();
            var valid = true;
            var outputs = component.getOutputPins();
            for (int i = 0; i < outputs.Length; i++)
            {
                if (outputs[i].state != test[i + k + 1])
                {
                    valid = false;
                    break;
                }
            }
            result[count] = valid;
            count++;
        }
        Console.WriteLine("test: ", component.getType());
        for (int i = 0; i < result.Length; i++)
        {
            Console.WriteLine("Test " + i + ": " + result[i]);
        }
    }
}