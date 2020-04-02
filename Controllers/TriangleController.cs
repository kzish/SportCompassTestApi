using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// handles the triangle endpoint
/// </summary>
namespace SportCompassRestApi.Controllers
{
    [Route("SportCompass/v1")]
    public class TriangleController : Controller
    {
        /// <summary>
        /// Enter 3 sides of a triangle a,b,c
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        [HttpGet("Triangle")]
        public string Triangle(double a, double b, double c)
        {
            //invalid trianlge any side less than or equal to 0
            if(a<=0 || b<=0 || c<=0)
            {
                return "Incorrect";
            }
            //scalene at least 2 sides equal
            if(a!=b  && b!=c && a!=c)
            {
                return "Scalene";
            }

            //equalateral all three sides equal
            else if(a==b && b==c && a==c)
            {
                return "Equilateral";
            }

            //isosceles at least 2 sides equal
            else if(a==b || b==c || a==c)
            {
                return "Isosceles";
            }
            //invalid
            else
            {
                return "Incorrect";
            }
        }
    }
}
