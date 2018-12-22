using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FamilyManager
{
    class function
    {
        //ساخت جدول زمانبندی
        public static Tuple<int,  int, int, DateTime> CreatTimeTable(int RePsubtractwidth, int HeightCanvas, int WidthCanvas, DateTime StartTime , DateTime EndTime,string canvasname)
        {
            int HeightObj = 0, StartWidth1Obj = 0 , EndWidth1Obj = 0;
            int MinuteStartTime = 0 , MinuteEndTime = 0;
            MinuteStartTime = (StartTime.Hour) * 60 + (StartTime.Minute);
            MinuteEndTime = (EndTime.Hour) * 60 + (EndTime.Minute);
            if (canvasname== "CanvasTimeTable1")
            {
                HeightObj = HeightCanvas / 8;
                StartWidth1Obj = (Convert.ToInt32(MinuteStartTime) - 6 * 60 * RePsubtractwidth) * WidthCanvas / (6 * 60);
                EndWidth1Obj = (Convert.ToInt32(MinuteEndTime) - 6 * 60 * RePsubtractwidth) * WidthCanvas / (6 * 60);
            }
            else
            {
                HeightObj = HeightCanvas / 4;
                StartWidth1Obj = (Convert.ToInt32(MinuteStartTime) - 12 * 60 * RePsubtractwidth) * WidthCanvas / (12 * 60);
                EndWidth1Obj = (Convert.ToInt32(MinuteEndTime) - 12 * 60 * RePsubtractwidth) * WidthCanvas / (12 * 60);
            }
            

           
        
            
            
            return Tuple.Create(HeightObj, StartWidth1Obj, EndWidth1Obj, EndTime); 
            
        }

    }
}
