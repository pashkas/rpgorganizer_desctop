using System;
using System.Collections.Generic;
using System.Linq;

namespace Sample.Model
{
    [Serializable]
    public class ChaLevAndValues
    {
        Pers _pers
        {
            get { return StaticMetods.PersProperty; }
        }

        public List<ChaLevAndValue> _chaLevAndValuesList;

        public ChaLevAndValues(Pers pers)
        {
        }

        ///// <summary>
        ///// ��������������, ������ � �� ��������
        ///// </summary>
        //public List<ChaLevAndValue> ChaLevAndValuesList
        //{
        //    get
        //    {
        //        if (_chaLevAndValuesList == null || !_chaLevAndValuesList.Any())
        //        {
        //            var l = new List<ChaLevAndValue>();
        //            for (int i = 0; i <= _pers.PersLevelProperty; i++)
        //            {
        //                AddChaLevValByLev(ref l, i);
        //            }
        //           _chaLevAndValuesList = l;
        //        }
        //        return _chaLevAndValuesList;
        //    }
        //    set
        //    {
        //        if (Equals(value, _chaLevAndValuesList)) return;
        //        _chaLevAndValuesList = value;
        //        _pers.OnPropertyChanged(nameof(ChaLevAndValuesList));
        //    }
        //}

        ///// <summary>
        ///// ����� ������� �������������� ��������� ������ �������� ������������� �� �������
        ///// </summary>
        ///// <param name="cha"></param>
        //public void ChaLevAndValuesListWhenDelCharact(Characteristic cha)
        //{
        //    var toDel = Enumerable.Where<ChaLevAndValue>(ChaLevAndValuesList, n => n.Guid == cha.GUID).ToList();
        //    foreach (var chaLevAndValue in toDel)
        //    {
        //        ChaLevAndValuesList.Remove(chaLevAndValue);
        //    }
        //}

        ///// <summary>
        ///// ����� ��������� ����� ��������������
        ///// </summary>
        ///// <param name="cha"></param>
        //public void ChaLevAndValuesListWhenAddCharact(Characteristic cha)
        //{
        //    //for (int i = 0; i <= _pers.PersLevelProperty; i++)
        //    //{
        //    //    if (i==0)
        //    //    {
        //    //        var firstVal = cha.FirstVal *2.0;
        //    //        ChaLevAndValuesList.Add(new ChaLevAndValue(i,cha.GUID,firstVal));
        //    //    }
        //    //    else
        //    //    {
        //    //        ChaLevAndValuesList.Add(new ChaLevAndValue(i, cha.GUID, cha.GetChaValue(true)));
        //    //    }
        //    //}
        //}

        ///// <summary>
        ///// �������� �������� � ������ ������������� ��� ����������� ������
        ///// </summary>
        ///// <param name="list">���� � ����������</param>
        ///// <param name="lvl">�������</param>
        //public void AddChaLevValByLev(ref List<ChaLevAndValue> list, int lvl)
        //{
        //    foreach (var characteristic in _pers.Characteristics)
        //    {
        //        var f = list.FirstOrDefault(n => n.Lev == lvl && n.Guid == characteristic.GUID);
        //        var firstVal = characteristic.FirstVal *2.0;
        //        if (f==null)
        //        {
        //            list.Add(lvl == 0
        //                ? new ChaLevAndValue(lvl, characteristic.GUID, firstVal)
        //                : new ChaLevAndValue(lvl, characteristic.GUID, characteristic.GetChaValue(true)));
        //        }
        //        else
        //        {
        //            f.Val = lvl == 0 ? firstVal : characteristic.GetChaValue(true);
        //        }
        //    }
        //}
    }
}