using System;
using System.Collections.Generic;
using System.Text;
using Training_App;
using View_Models;

namespace MainIenterfaces
{
    public interface ITrainingInfo
    {
        List<TrainingListVM> GetTrainingDetails();
        bool InsertTrainingDetails(TrainingVM trainer);
        bool UpdateTrainingDetails(int id, TrainingVM trainer);
        bool DeleteTrainingDetails(int id);
    }
}
