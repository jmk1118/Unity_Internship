using UnityEngine;
using System;

/// <summary>
/// by MK, csv파일에서 읽어들인 meta data들을 가지고 있는 클래스
/// </summary>
public class CSV_Data : MonoBehaviour 
{
    [SerializeField] string PROJ_NO; // 프로젝트 넘버
    [SerializeField] string EQP_KEY; // 제품 키
    [SerializeField] string Equip_Type; // 제품 타입
    [SerializeField] string OID; //
    [SerializeField] double COG_X; // x좌표
    [SerializeField] double COG_Y; // y좌표
    [SerializeField] double COG_Z; // z좌표

    // 조명 SAT 오브젝트에 csv에서 읽어들인 meta date들을 입력하는 함수
    public void AddData(string proj_no, string eqp_key, string equip_type, string oid, string cog_x, string cog_y, string cog_z)
    {
        PROJ_NO = proj_no;
        EQP_KEY = eqp_key;
        Equip_Type = equip_type;
        OID = oid;
        COG_X = Math.Round(double.Parse(cog_x), 3);
        COG_Y = Math.Round(double.Parse(cog_y), 3);
        COG_Z = Math.Round(double.Parse(cog_z), 3);
    }

    // 입력된 meta data의 포지션대로 조명 SAT 오브젝트를 이동시키는 함수
    public Vector3 ChangePosition()
    {
        return new Vector3(Convert.ToSingle(COG_X), Convert.ToSingle(COG_Z), Convert.ToSingle(COG_Y));
    }

    // 입력된 meta data에서 IES 파일 이름을 반환하는 함수
    public String WhatEquipType()
    {
        return Equip_Type;
    }
}