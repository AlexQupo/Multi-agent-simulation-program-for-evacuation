
public class Function
{
    public double K { get; set; }
    public double B { get; set; }

    public Point P1 { get; set; }//точка старта функции
    public Point P2 { get; set; }//точка конца функции

    public Function(Point p1, Point p2)
    {
        P1 = p1;
        P2 = p2;
        K = (p2.Y - p1.Y) / (p2.X - p1.X);
        B = p2.Y - K * p2.X;
    }

    /// <summary>
    /// Находит, в каком месте функция, к которой обращаются, пересчет высоту y
    /// </summary>
    /// <param name="y"></param>
    /// <returns></returns>
    public double GetValueX(double y)
    {
        return (y - this.B) / this.K;
    }
}