
using System;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;

namespace Parser
{
    class MainProgram
    {
        public string strip(string prog)
        {

            string result = string.Concat(prog.Where(c => !char.IsWhiteSpace(c)));
            return result;
        }

        static void Main(string[] args)
        {

            //var prg = "void Bv2(bool XI, bool PY){if (RHM(true, lV)){int VHi;}bool yFM;}else{}if (!false){if (BZX = !(PQ = o) > !123){}else{    return false;}false;bool BE;}else{}return 1;}";
            var stdin = new StreamReader(Console.OpenStandardInput());
           var prg = stdin.ReadToEnd();
             //var prg = "void QqN(bool Mbx){}int FhC(int Z){}bool N5A(int cq, int gtr){{if (Wb(nW)){{{if (false){if (true){FDy = false - (TCE = lGd() - gn);}else{if (dfs(Qi(QJP) > 636, M(VK(), 272 || true * 50))){}else{while (false){return -(!(4 != (I = true)) / (N = 61 - HIY(!((X = -2) != (86 == Nh) == fh8) >= Fhn(s, l = 5), wu) >= true)) != n(false, 622 / (55 <= true), true);}}}}else{}{{while ((C = RwO) - true){return XA;}raL(W8, K0 = DMb(POy(Ph = o = true <= 5, Goy > f(K8 = w)), 68) + -(D6C = (r = 75) == m), i = 8);}return 148 <= !(Jk0 = Uij(Je));bool unA;}}{{while (false){!!vi2(-true != nz, y4m, G = Zg);bool bbh;}}if (Wz8 = WLm){if (hn(NKX(false < true, L5 * x8(Fe(), 12, 5), D = eF(!(mre = t8z = true)) && 3), 815, false != 76)){while (id = U = ZL = 88){{return HMG;{return PUj() >= (o = RnY(T3 = true) && gj);}while (true > e(xxD(), false)){if (cqv = -h6){}else{}bool ca;}}}if (true){Zw;return true;}else{}}else{}return ov1;return !(wln = O(R / X, 7));}else{}}}Npv = kq0;return y = sj = n = 6;}else{j = true;{}int xJ;}}}";
            // var prg = "int U(bool l){                while (!false)                {                    bool V5;                    return UP8;                    {                        {                            if (511)                            {                                int hyt;                            }                            else                            {                            }                            while (I1F(2, true))                            {                            }                            return 36;                        }                        if (W(--(c = 308), false, wIC()))                        {                            if (false)                            {                                bool vn;                                bool w;                                {                                }                            }                            else                            {                                int ui;                                while (qIV)                                {                                    if (K = (b3R = IY()) < J0(8, i4 = A(4, 745, true), 7))                                    {                                        return false;                                        bool we;                                    }                                    else                                    {                                    }                                    return uu;                                    tf = -(S = bhQ);                                }                            }                        }                        else                        {                        }                        {                        }                    }                }            }            int q()            {                return FZm = b1 = (P = g = fh = true) - (T7N = xp(true, Q(!true, N8), kn = n8(XO = i = true)));            }            int oj5()            {                if (Jt3(-278))                {                }                else                {                    if (rW1)                    {                        44;                        if (43)                        {                            return h3G(Og = !WQ(L(J(true, 14 <= 26 != uv(-dpU(), !(O = false) <= iy(true) == o(false), true) != (eZ7 = -(true <= ZT(-!true, 47))), T = NOn) > Z) < ep(), re, AM), f = D6() <= (F = !84), o);                            L;                        }                        else                        {                        }                    }                    else                    {                    }                }            }            ";
            //var prg = "void f(){ 270 || true*50; }";
            byte[] data = Encoding.ASCII.GetBytes(prg);
            MemoryStream stream = new MemoryStream(data, 0, data.Length);
            Scanner lexer = new Scanner(stream);
            Parser parser = new Parser(lexer);
            var success = parser.Parse();
            var b = new PrettyBuilder();
            parser.Program.Pretty(b);

       
            if (success)
            {
            
                string resultPRG = string.Concat(prg.Where(c => !char.IsWhiteSpace(c)));
                string resultFirst = string.Concat(b.ToString().Where(c => !char.IsWhiteSpace(c)));

          

                    if (resultFirst.Equals(resultPRG))
                    {
                        Console.WriteLine("True");
                    }
                else
                {
                   for(int i = 0; i < resultFirst.Length; i++)
                    {
                        if(resultFirst[i] != resultPRG[i])
                        {
                            Console.WriteLine("ParserString had: " + resultFirst[i] + " at stringindex: " + i);
                            Console.Write("In the context of: ");
                            for (int y = i; y < resultFirst.Length; y++)
                            {
                                Console.Write(resultFirst[y]);
                            }
                            Console.WriteLine("InputString had: " + resultPRG[i] + " at stringindex: " + i);
                            Console.Write("In the context of: ");
                            for(int z = i; z < resultPRG.Length; z++)
                            {
                                Console.Write(resultPRG[z]);
                            }
                            break;
                        }
                    }
                }
             }


               

        }
    }
}


