using System;
using System.Collections.Generic;
using System.Linq;

public class Manager
{
    public List<double> y = new List<double>(); //список значений у(х)
    public List<double> x = new List<double>(); 
    public List<Rule> _rules = new List<Rule>();
    List<Rule> _rulesOfFire = new List<Rule>();
    List<Rule> _rulesOfDavka = new List<Rule>();
    List<Rule> _rulesOfSkopleniye = new List<Rule>();
    List<double> _weightOfFireRules = new List<double>();
    List<double> _weightOfDavkaRules = new List<double>();
    List<double> _weightOfSkopleniyeRules = new List<double>();
    public List<Rule> _currentRules = new List<Rule>();
    List<double> _currentWeight = new List<double>();

    public void MakeRules()
    {
        ListOfRules();
        int n = 10000;
        for (int xi = 0; xi < n; xi++)
        {
            double curX = xi;
            x.Add(curX);
        }
    }

    List<Rule> GetListofRules(TypeOfSituations tos)
    {
        switch (tos)
        {
            case TypeOfSituations.Fire:
                return _rulesOfFire;

            case TypeOfSituations.Crowd:
                return _rulesOfDavka;

            case TypeOfSituations.Congestion:
                return _rulesOfSkopleniye;
            default:
                return null;
        }
    }
    List<double> GetListofWeights(TypeOfSituations tos)
    {
        switch (tos)
        {
            case TypeOfSituations.Fire:
                return _weightOfFireRules;

            case TypeOfSituations.Crowd:
                return _weightOfDavkaRules;

            case TypeOfSituations.Congestion:
                return _weightOfSkopleniyeRules;
            default:
                return null;
        }
    }

    double CheckWeight(double fst_situation, TypeOfSituations fst_type, double snd_situation, TypeOfSituations snd_type)
    {
        List<Rule> fstList = GetListofRules(fst_type);
        List<Rule> sndList = GetListofRules(snd_type);
        List<double> fstWeights = GetListofWeights(fst_type);
        List<double> sndWeights = GetListofWeights(snd_type);
        double fstY = 0;
        double sndY = 0;
        List<double> fstYs = new List<double>();
        List<double> sndYs = new List<double>();

        for (int i = 0; i < fstList.Count; i++)
        {
            fstYs.Add(fstList[i].grafIf2.GetValueY(fst_situation) * fstWeights[i]);
        }
        for (int i = 0; i < sndList.Count; i++)
        {
            sndYs.Add(sndList[i].grafIf2.GetValueY(snd_situation) * sndWeights[i]);
        }

        fstY = fstYs.Max();
        sndY = sndYs.Max();

        if (fstY > sndY)
            return fst_situation;
        else
            return snd_situation;


    }
    double CheckWeight(double fst_situation, TypeOfSituations fst_type, double snd_situation, TypeOfSituations snd_type, double thd_situation, TypeOfSituations thd_type)
    {
        List<Rule> fstList = GetListofRules(fst_type);
        List<Rule> sndList = GetListofRules(snd_type);
        List<Rule> thdList = GetListofRules(thd_type);
        List<double> fstWeights = GetListofWeights(fst_type);
        List<double> sndWeights = GetListofWeights(snd_type);
        List<double> thdWeights = GetListofWeights(thd_type);
        double fstY = 0;
        double sndY = 0;
        double thdY = 0;
        List<double> fstYs = new List<double>();
        List<double> sndYs = new List<double>();
        List<double> thdYs = new List<double>();

        for (int i = 0; i < fstList.Count; i++)
        {
            fstYs.Add(fstList[i].grafIf2.GetValueY(fst_situation) * fstWeights[i]);
        }
        for (int i = 0; i < sndList.Count; i++)
        {
            sndYs.Add(sndList[i].grafIf2.GetValueY(snd_situation) * sndWeights[i]);
        }
        for (int i = 0; i < thdList.Count; i++)
        {
            thdYs.Add(thdList[i].grafIf2.GetValueY(thd_situation) * thdWeights[i]);
        }

        fstY = fstYs.Max();
        sndY = sndYs.Max();
        thdY = thdYs.Max();

        if (fstY > sndY)
            if (fstY > thdY)
                return fst_situation;
            else
                return thd_situation;
        else if (sndY > thdY)
            return snd_situation;
        else
            return thd_situation;
    }


    public void CalculateResult(double[] characters, double situation, TypeOfSituations type)
    {
        y.Clear();
        double character = 0;
        if (type == TypeOfSituations.Fire)
        {
            _currentRules = _rulesOfFire;
            _currentWeight = _weightOfFireRules;
            character = characters[0];
        }
        else if (type == TypeOfSituations.Crowd)
        {
            _currentRules = _rulesOfDavka;
            _currentWeight = _weightOfDavkaRules;
            character = characters[1];
        }

        else if (type == TypeOfSituations.Congestion)
        {
            _currentRules = _rulesOfSkopleniye;
            _currentWeight = _weightOfSkopleniyeRules;
            character = characters[2];
        }

        GetResultUsingKnowledge(character, situation);
    }

    public void CalculateResult(double[] characters, double fst_situation, TypeOfSituations fst_type, double snd_situation, TypeOfSituations snd_type)
    {
        y.Clear();
        double character = 0;
        double currentSituation = CheckWeight(fst_situation, fst_type, snd_situation, snd_type);
        if (currentSituation == fst_situation)
        {
            if (fst_type == TypeOfSituations.Fire)
            {
                _currentRules = _rulesOfFire;
                _currentWeight = _weightOfFireRules;
                character = characters[0];
            }
            else if (fst_type == TypeOfSituations.Crowd)
            {
                _currentRules = _rulesOfDavka;
                _currentWeight = _weightOfDavkaRules;
                character = characters[1];
            }

            else if (fst_type == TypeOfSituations.Congestion)
            {
                _currentRules = _rulesOfSkopleniye;
                _currentWeight = _weightOfSkopleniyeRules;
                character = characters[2];
            }

        }
        else if (currentSituation == snd_situation)
        {
            if (snd_type == TypeOfSituations.Fire)
            {
                _currentRules = _rulesOfFire;
                _currentWeight = _weightOfFireRules;
                character = characters[0];
            }
            else if (snd_type == TypeOfSituations.Crowd)
            {
                _currentRules = _rulesOfDavka;
                _currentWeight = _weightOfDavkaRules;
                character = characters[1];
            }

            else if (snd_type == TypeOfSituations.Congestion)
            {
                _currentRules = _rulesOfSkopleniye;
                _currentWeight = _weightOfSkopleniyeRules;
                character = characters[2];
            }
        }

        GetResultUsingKnowledge(character, currentSituation);
    }

    public void CalculateResult(double[] characters, double fst_situation, TypeOfSituations fst_type, double snd_situation, TypeOfSituations snd_type, double thd_situation, TypeOfSituations thd_type)
    {
        y.Clear();
        double character = 0;
        double currentSituation = CheckWeight(fst_situation, fst_type, snd_situation, snd_type, thd_situation, thd_type);
        if (currentSituation == fst_situation)
        {
            if (fst_type == TypeOfSituations.Fire)
            {
                _currentRules = _rulesOfFire;
                _currentWeight = _weightOfFireRules;
                character = characters[0];
            }
            else if (fst_type == TypeOfSituations.Crowd)
            {
                _currentRules = _rulesOfDavka;
                _currentWeight = _weightOfDavkaRules;
                character = characters[1];
            }

            else if (fst_type == TypeOfSituations.Congestion)
            {
                _currentRules = _rulesOfSkopleniye;
                _currentWeight = _weightOfSkopleniyeRules;
                character = characters[2];
            }
        }
        else if (currentSituation == snd_situation)
        {
            if (snd_type == TypeOfSituations.Fire)
            {
                _currentRules = _rulesOfFire;
                _currentWeight = _weightOfFireRules;
                character = characters[0];
            }
            else if (snd_type == TypeOfSituations.Crowd)
            {
                _currentRules = _rulesOfDavka;
                _currentWeight = _weightOfDavkaRules;
                character = characters[1];
            }

            else if (snd_type == TypeOfSituations.Congestion)
            {
                _currentRules = _rulesOfSkopleniye;
                _currentWeight = _weightOfSkopleniyeRules;
                character = characters[2];
            }
        }
        else if (currentSituation == thd_situation)
        {
            if (thd_type == TypeOfSituations.Fire)
            {
                _currentRules = _rulesOfFire;
                _currentWeight = _weightOfFireRules;
                character = characters[0];
            }
            else if (thd_type == TypeOfSituations.Crowd)
            {
                _currentRules = _rulesOfDavka;
                _currentWeight = _weightOfDavkaRules;
                character = characters[1];
            }

            else if (thd_type == TypeOfSituations.Congestion)
            {
                _currentRules = _rulesOfSkopleniye;
                _currentWeight = _weightOfSkopleniyeRules;
                character = characters[2];
            }
        }
        GetResultUsingKnowledge(character, currentSituation);
    }

    public TypeOfSituations GetTypeOfSituation()
    {
        if(_currentRules == _rulesOfFire)
            return TypeOfSituations.Fire;
        else if(_currentRules == _rulesOfDavka)
            return TypeOfSituations.Crowd;
        else
            return TypeOfSituations.Congestion;
    }

    public double CountMax()
    {
        bool f = true;
        int maxEndIndex = 0;
        for (int yi = 0; yi < y.Count; yi++)
        {
            if (y[yi] != 0)
                f = false;
            if (y[yi] >= y[maxEndIndex])
            {
                maxEndIndex = yi;
            }
        }
        int maxStartIndex = maxEndIndex;
        while (y[maxStartIndex] == y[maxEndIndex])
        {
            if (maxStartIndex == 0)
                break;
            maxStartIndex--;
        }
        maxStartIndex++;

        double zMax = (maxStartIndex + maxEndIndex) / 2.0;
        if (f)
            return -1;
        return zMax;
    }

    public double CountCentroid()
    {
        double zCentreChislitel = 0;
        double zCentreZnamenatel = 0;
        for (int i = 0; i < y.Count; i++)
        {
            zCentreChislitel += x[i] * y[i];
            zCentreZnamenatel += y[i];
        }
        double zCentroid = zCentreChislitel / zCentreZnamenatel;
        return zCentroid;
    }

    void ListOfRules()
    {
        
        /////////
        ///много средне мало
        ///
        #region Characteristics
        List<Function> _funcsCharacter1 = new List<Function>();
        _funcsCharacter1.Add(new Function(new Point(0, 1), new Point(17, 1)));
        _funcsCharacter1.Add(new Function(new Point(17, 1), new Point(40, 0)));
        //_funcsCharacter1.Add(new Function(new Point(40, 0), new Point(100, 0)));
        Grafic gr3_character = new Grafic(_funcsCharacter1);

        List<Function> _funcsCharacter2 = new List<Function>();
        //_funcsCharacter2.Add(new Function(new Point(0, 0), new Point(25, 0)));
        _funcsCharacter2.Add(new Function(new Point(25, 0), new Point(45, 1)));
        _funcsCharacter2.Add(new Function(new Point(45, 1), new Point(55, 1)));
        _funcsCharacter2.Add(new Function(new Point(55, 1), new Point(75, 0)));
        //_funcsCharacter2.Add(new Function(new Point(75, 0), new Point(100, 0)));
        Grafic gr2_character = new Grafic(_funcsCharacter2);

        List<Function> _funcsCharacter3 = new List<Function>();
        //_funcsCharacter3.Add(new Function(new Point(0, 0), new Point(60, 0)));
        _funcsCharacter3.Add(new Function(new Point(60, 0), new Point(84, 1)));
        _funcsCharacter3.Add(new Function(new Point(84, 1), new Point(100, 1)));
        Grafic gr1_character = new Grafic(_funcsCharacter3);

        #endregion

        /////////
        ///Растерянный обычный целеустремленный
        /////////
        ///Огонь
        ///Слабый Средний Мощный
        ///
        #region Fire
        List<Function> _funcsSituation1 = new List<Function>();
        _funcsSituation1.Add(new Function(new Point(0, 1), new Point(15, 1)));
        _funcsSituation1.Add(new Function(new Point(15, 1), new Point(30, 0)));
        //_funcsSituation1.Add(new Function(new Point(30, 0), new Point(100, 0)));
        Grafic gr1_situation = new Grafic(_funcsSituation1);

        List<Function> _funcsSituation2 = new List<Function>();
        //_funcsSituation2.Add(new Function(new Point(0, 0), new Point(25, 0)));
        _funcsSituation2.Add(new Function(new Point(25, 0), new Point(35, 1)));
        _funcsSituation2.Add(new Function(new Point(35, 1), new Point(65, 1)));
        _funcsSituation2.Add(new Function(new Point(65, 1), new Point(75, 0)));
        //_funcsSituation2.Add(new Function(new Point(75, 0), new Point(100, 0)));
        Grafic gr2_situation = new Grafic(_funcsSituation2);

        List<Function> _funcsSituation3 = new List<Function>();
        //_funcsSituation3.Add(new Function(new Point(0, 0), new Point(60, 0)));
        _funcsSituation3.Add(new Function(new Point(60, 0), new Point(80, 1)));
        _funcsSituation3.Add(new Function(new Point(80, 1), new Point(100, 1)));
        Grafic gr3_situation = new Grafic(_funcsSituation3);

        //результат
        List<Function> _funcsResult1 = new List<Function>();
        _funcsResult1.Add(new Function(new Point(0, 1), new Point(10, 1)));
        _funcsResult1.Add(new Function(new Point(10, 1), new Point(20, 0)));
        //_funcsResult1.Add(new Function(new Point(20, 0), new Point(100, 0)));
        Grafic gr1_result = new Grafic(_funcsResult1);

        List<Function> _funcsResult2 = new List<Function>();
        //_funcsResult2.Add(new Function(new Point(0, 0), new Point(10, 0)));
        _funcsResult2.Add(new Function(new Point(10, 0), new Point(25, 1)));
        _funcsResult2.Add(new Function(new Point(25, 1), new Point(40, 0)));
        //_funcsResult2.Add(new Function(new Point(40, 0), new Point(100, 0)));
        Grafic gr2_result = new Grafic(_funcsResult2);

        List<Function> _funcsResult3 = new List<Function>();
        //_funcsResult3.Add(new Function(new Point(0, 0), new Point(30, 0)));
        _funcsResult3.Add(new Function(new Point(30, 0), new Point(50, 1)));
        _funcsResult3.Add(new Function(new Point(50, 1), new Point(70, 0)));
        //_funcsResult3.Add(new Function(new Point(70, 0), new Point(100, 0)));
        Grafic gr3_result = new Grafic(_funcsResult3);

        List<Function> _funcsResult4 = new List<Function>();
        //_funcsResult4.Add(new Function(new Point(0, 0), new Point(60, 0)));
        _funcsResult4.Add(new Function(new Point(60, 0), new Point(75, 1)));
        _funcsResult4.Add(new Function(new Point(75, 1), new Point(90, 0)));
        //_funcsResult4.Add(new Function(new Point(90, 0), new Point(100, 0)));
        Grafic gr4_result = new Grafic(_funcsResult4);

        List<Function> _funcsResult5 = new List<Function>();
        //_funcsResult5.Add(new Function(new Point(0, 0), new Point(80, 0)));
        _funcsResult5.Add(new Function(new Point(80, 0), new Point(90, 1)));
        _funcsResult5.Add(new Function(new Point(90, 1), new Point(100, 1)));
        Grafic gr5_result = new Grafic(_funcsResult5);


        Rule rule1_1 = new Rule(gr1_character, gr1_situation, gr1_result);
        _rulesOfFire.Add(rule1_1);
        _weightOfFireRules.Add(0.9);

        Rule rule1_2 = new Rule(gr1_character, gr2_situation, gr2_result);
        _rulesOfFire.Add(rule1_2);
        _weightOfFireRules.Add(0.9);

        Rule rule1_3 = new Rule(gr1_character, gr3_situation, gr2_result);
        _rulesOfFire.Add(rule1_3);
        _weightOfFireRules.Add(0.9);

        Rule rule1_4 = new Rule(gr2_character, gr1_situation, gr2_result);
        _rulesOfFire.Add(rule1_4);
        _weightOfFireRules.Add(0.9);

        Rule rule1_5 = new Rule(gr2_character, gr2_situation, gr3_result);
        _rulesOfFire.Add(rule1_5);
        _weightOfFireRules.Add(0.9);

        Rule rule1_6 = new Rule(gr2_character, gr3_situation, gr4_result);
        _rulesOfFire.Add(rule1_6);
        _weightOfFireRules.Add(0.9);

        Rule rule1_7 = new Rule(gr3_character, gr1_situation, gr4_result);
        _rulesOfFire.Add(rule1_7);
        _weightOfFireRules.Add(0.9);

        Rule rule1_8 = new Rule(gr3_character, gr2_situation, gr4_result);
        _rulesOfFire.Add(rule1_8);
        _weightOfFireRules.Add(0.9);

        Rule rule1_9 = new Rule(gr3_character, gr3_situation, gr5_result);
        _rulesOfFire.Add(rule1_9);
        _weightOfFireRules.Add(0.9);
        #endregion
        ///////////
        ///Храбрый Пугливый
        //Малая средняя большая
        #region Crowd
        List<Function> _funcsSituation4 = new List<Function>();
        _funcsSituation4.Add(new Function(new Point(0, 1), new Point(1, 1)));
        _funcsSituation4.Add(new Function(new Point(1, 1), new Point(3, 0)));
        //_funcsSituation4.Add(new Function(new Point(3, 0), new Point(10, 0)));
        Grafic gr4_situation = new Grafic(_funcsSituation4);

        List<Function> _funcsSituation5 = new List<Function>();
        //_funcsSituation5.Add(new Function(new Point(0, 0), new Point(2, 0)));
        _funcsSituation5.Add(new Function(new Point(2, 0), new Point(4, 1)));
        _funcsSituation5.Add(new Function(new Point(4, 1), new Point(6, 0)));
        //_funcsSituation5.Add(new Function(new Point(6, 0), new Point(10, 0)));
        Grafic gr5_situation = new Grafic(_funcsSituation5);

        List<Function> _funcsSituation6 = new List<Function>();
        //_funcsSituation6.Add(new Function(new Point(0, 0), new Point(5, 0)));
        _funcsSituation6.Add(new Function(new Point(5, 0), new Point(8, 1)));
        _funcsSituation6.Add(new Function(new Point(8, 1), new Point(10, 1)));
        Grafic gr6_situation = new Grafic(_funcsSituation6);

        //Результаты

        List<Function> _funcsResult6 = new List<Function>();
        _funcsResult6.Add(new Function(new Point(0, 0), new Point(20, 1)));
        _funcsResult6.Add(new Function(new Point(20, 1), new Point(40, 0)));
        //_funcsResult6.Add(new Function(new Point(40, 0), new Point(100, 0)));
        Grafic gr6_result = new Grafic(_funcsResult6);

        List<Function> _funcsResult7 = new List<Function>();
        //_funcsResult7.Add(new Function(new Point(0, 0), new Point(30, 0)));
        _funcsResult7.Add(new Function(new Point(30, 0), new Point(50, 1)));
        _funcsResult7.Add(new Function(new Point(50, 1), new Point(70, 0)));
        //_funcsResult7.Add(new Function(new Point(70, 0), new Point(100, 0)));
        Grafic gr7_result = new Grafic(_funcsResult7);

        List<Function> _funcsResult8 = new List<Function>();
        //_funcsResult8.Add(new Function(new Point(0, 0), new Point(60, 0)));
        _funcsResult8.Add(new Function(new Point(60, 0), new Point(80, 1)));
        _funcsResult8.Add(new Function(new Point(80, 1), new Point(100, 0)));
        Grafic gr8_result = new Grafic(_funcsResult8);



        Rule rule2_1 = new Rule(gr1_character, gr5_situation, gr6_result);
        _rulesOfDavka.Add(rule2_1);
        _weightOfDavkaRules.Add(0.5);

        Rule rule2_2 = new Rule(gr1_character, gr6_situation, gr6_result);
        _rulesOfDavka.Add(rule2_2);
        _weightOfDavkaRules.Add(0.5);

        Rule rule2_3 = new Rule(gr3_character, gr4_situation, gr7_result);
        _rulesOfDavka.Add(rule2_3);
        _weightOfDavkaRules.Add(0.5);

        Rule rule2_4 = new Rule(gr3_character, gr5_situation, gr7_result);
        _rulesOfDavka.Add(rule2_4);
        _weightOfDavkaRules.Add(0.5);

        Rule rule2_5 = new Rule(gr3_character, gr6_situation, gr8_result);
        _rulesOfDavka.Add(rule2_5);
        _weightOfDavkaRules.Add(0.5);

        #endregion
        ///////////
        ///Самоуверенный обычный неуверенный
        ///
        ///Малая средняя большая
        ///
        #region Congestion
        List<Function> _funcsSituation7 = new List<Function>();
        _funcsSituation7.Add(new Function(new Point(0, 1), new Point(6, 1)));
        _funcsSituation7.Add(new Function(new Point(6, 1), new Point(10, 0)));
        //_funcsSituation7.Add(new Function(new Point(10, 0), new Point(25, 0)));

        Grafic gr7_situation = new Grafic(_funcsSituation7);

        List<Function> _funcsSituation8 = new List<Function>();
        //_funcsSituation8.Add(new Function(new Point(0, 0), new Point(8, 0)));
        _funcsSituation8.Add(new Function(new Point(8, 0), new Point(12, 1)));
        _funcsSituation8.Add(new Function(new Point(12, 1), new Point(16, 1)));
        _funcsSituation8.Add(new Function(new Point(16, 1), new Point(20, 0)));
        //_funcsSituation8.Add(new Function(new Point(20, 0), new Point(25, 0)));
        Grafic gr8_situation = new Grafic(_funcsSituation8);

        List<Function> _funcsSituation9 = new List<Function>();
        //_funcsSituation9.Add(new Function(new Point(0, 0), new Point(18, 0)));
        _funcsSituation9.Add(new Function(new Point(18, 0), new Point(22, 1)));
        _funcsSituation9.Add(new Function(new Point(22, 1), new Point(25, 1)));
        Grafic gr9_situation = new Grafic(_funcsSituation9);

        //Результат

        List<Function> _funcsResult9 = new List<Function>();
        _funcsResult9.Add(new Function(new Point(0, 1), new Point(25, 1)));
        _funcsResult9.Add(new Function(new Point(25, 1), new Point(45, 0)));
        //_funcsResult9.Add(new Function(new Point(45, 0), new Point(100, 0)));
        Grafic gr9_result = new Grafic(_funcsResult9);

        List<Function> _funcsResult10 = new List<Function>();
        //_funcsResult10.Add(new Function(new Point(0, 0), new Point(35, 0)));
        _funcsResult10.Add(new Function(new Point(35, 0), new Point(40, 1)));
        _funcsResult10.Add(new Function(new Point(40, 1), new Point(60, 1)));
        _funcsResult10.Add(new Function(new Point(60, 1), new Point(65, 0)));
        //_funcsResult10.Add(new Function(new Point(65, 0), new Point(100, 0)));
        Grafic gr10_result = new Grafic(_funcsResult10);

        List<Function> _funcsResult11 = new List<Function>();
        //_funcsResult11.Add(new Function(new Point(0, 0), new Point(55, 0)));
        _funcsResult11.Add(new Function(new Point(55, 0), new Point(75, 1)));
        _funcsResult11.Add(new Function(new Point(75, 1), new Point(100, 1)));
        Grafic gr11_result = new Grafic(_funcsResult11);



        Rule rule3_1 = new Rule(gr1_character, new Grafic(new List<Function>() { new Function(new Point(0, 1), new Point(25, 1)) }), gr9_result);//ОСОБЕННОЕ ПРАВИЛО
        _rulesOfSkopleniye.Add(rule3_1);
        _weightOfSkopleniyeRules.Add(0.3);

        Rule rule3_2 = new Rule(gr2_character, gr7_situation, gr9_result);
        _rulesOfSkopleniye.Add(rule3_2);
        _weightOfSkopleniyeRules.Add(0.3);

        Rule rule3_3 = new Rule(gr2_character, gr8_situation, gr9_result);
        _rulesOfSkopleniye.Add(rule3_3);
        _weightOfSkopleniyeRules.Add(0.3);

        Rule rule3_4 = new Rule(gr2_character, gr9_situation, gr11_result);
        _rulesOfSkopleniye.Add(rule3_4);
        _weightOfSkopleniyeRules.Add(0.3);

        Rule rule3_5 = new Rule(gr3_character, gr7_situation, gr10_result);
        _rulesOfSkopleniye.Add(rule3_5);
        _weightOfSkopleniyeRules.Add(0.3);

        Rule rule3_6 = new Rule(gr3_character, gr8_situation, gr10_result);
        _rulesOfSkopleniye.Add(rule3_6);
        _weightOfSkopleniyeRules.Add(0.3);

        Rule rule3_7 = new Rule(gr3_character, gr9_situation, gr11_result);
        _rulesOfSkopleniye.Add(rule3_7);
        _weightOfSkopleniyeRules.Add(0.3);

        #endregion

        _rules.AddRange(_rulesOfFire);
        _rules.AddRange(_rulesOfDavka);
        _rules.AddRange(_rulesOfSkopleniye);

    }

    public void GetResultUsingKnowledge(double character, double situation)
    {
        List<Grafic> grafsWithOtsech = MakeListGrafsWithOtsech(character, situation);

        double n = 10000;

        for (int i = 0; i < n; i++)
            y.Add(0);
        //находим значение y 
        for (int xi = 0; xi < n; xi++)
        {
            double curX = xi;
            foreach (Grafic graf in grafsWithOtsech)
                if (graf.GetValueY(curX) > y[xi])
                    y[xi] = graf.GetValueY(curX);
        }
    }

    public List<Grafic> MakeListGrafsWithOtsech(double character, double situation)
    {
        List<Grafic> grafsWithOtsech = new List<Grafic>();

        for (int i = 0; i < _currentRules.Count; i++)
        {
            double highChar = _currentRules[i].grafIf1.GetValueY(character);
            double highSituat = _currentRules[i].grafIf2.GetValueY(situation) * _currentWeight[i];
            double highRes = Math.Min(highChar, highSituat);

            Grafic grafWithOtsech = _currentRules[i].grafRes.MakeOtsech(highRes);
            grafsWithOtsech.Add(grafWithOtsech);
            _currentRules[i].grafResWithOtsech = grafWithOtsech;
        }
        return grafsWithOtsech;
    }
}
