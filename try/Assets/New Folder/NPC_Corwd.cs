using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Crowd
{
    public class vec {
        public double x;
        public double y;
        }
    public class NPC_Parameter
    {

        public vec posi;
        public vec velo;
        public double physical_par;
        public double max_len;
        public double face_angle;
        public double dis_angle;
    }
    public class calculate
    {
        static public vec v(NPC_Parameter A, NPC_Parameter B)
        {
            vec angle=new vec();
            angle.x = A.posi.x - B.posi.x;
            angle.y = A.posi.y - B.posi.y;
            double lens = Math.Pow(angle.x* angle.x+ angle.y* angle.y,0.5);
            double o = Math.Acos(angle.x / lens);
            double P = o - A.dis_angle;
            double Q = o - B.face_angle;
            double D = A.physical_par * (1 + Math.Cos(P)) / (4 * B.physical_par * lens);
            angle.x /= lens*D;
            angle.y /= lens*D;
            return (angle);
        }
        static public NPC_Parameter initital(double x,double y,double face_angle)
        {
            NPC_Parameter neww = new NPC_Parameter();
            System.Random R = new System.Random();
            neww.dis_angle = 0;
            neww.face_angle = face_angle;
            neww.physical_par = new double();
            neww.physical_par = R.Next(0, 1000) / 1000 + 0.5;
            neww.posi = new vec();
            neww.posi.x = x;
            neww.posi.y = y;
            neww.velo = new vec();
            neww.velo.x = 0;
            neww.velo.y = 0;
            neww.max_len = R.Next(0, 1000) / 500 + 0.5;
            neww.dis_angle = 0;
            return (neww);
        }
        static public vec obstacle(vec posi, vec posi2, double rate)
        {
            vec L = new vec();
            L.x = posi.x - posi2.x;
            L.y = posi.y - posi2.y;
            double lens = Math.Pow(L.x * L.x + L.y * L.y, 0.5);
            L.x = rate / lens * L.x;
            L.y = rate / lens * L.y;
            return (L);
        }
    }
}

