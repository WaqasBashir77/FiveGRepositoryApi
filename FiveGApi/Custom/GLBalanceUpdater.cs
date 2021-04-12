using FiveGApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FiveGApi.Custom
{
    public static class GLBalanceUpdater
    {
        private static MIS_DBEntities1 db = new MIS_DBEntities1();

        public static void UpdateGLBalances(int H_ID, User userSecurityGroup)
        {
            var GLLines = db.GL_Lines.Where(x => x.H_ID == H_ID).AsQueryable().ToList();
            foreach (var gllines in GLLines)
            {
                var glBalance = db.GL_Balances.Where(x => x.C_CODE == gllines.C_CODE).AsQueryable().FirstOrDefault();
                if (glBalance != null)
                {
                    if (gllines.Credit != null)
                    {
                        glBalance.Credit = glBalance.Credit + gllines.Credit;
                    }
                    if (gllines.Debit != null)
                    {
                        glBalance.Debit = glBalance.Debit + gllines.Debit;

                    }
                    glBalance.Effect_Trans_ID = gllines.L_ID.ToString();
                    glBalance.Updated_By = userSecurityGroup.UserName; ;
                    glBalance.Updated_On = DateTime.Now;
                    db.SaveChanges();
                }
                else
                {
                    GL_Balances gL_Balances = new GL_Balances();
                    if (gllines.Credit != null)
                    {
                        gL_Balances.Credit = gllines.Credit;
                    }
                    if (gllines.Debit != null)
                    {
                        gL_Balances.Debit = gllines.Debit;

                    }
                    //gL_Balances.Debit = gllines.Debit;
                    gL_Balances.C_CODE = gllines.C_CODE;
                    gL_Balances.Bal_Date = DateTime.Now;
                    gL_Balances.Effect_Trans_ID = gllines.L_ID.ToString();
                    gL_Balances.Created_By = userSecurityGroup.UserName; ;
                    gL_Balances.Created_On = DateTime.Now;
                    db.GL_Balances.Add(gL_Balances);
                    db.SaveChanges();
                }
            }
        }
    }
}