﻿// Accord Statistics Library
// The Accord.NET Framework
// http://accord-net.origo.ethz.ch
//
// Copyright © César Souza, 2009-2012
// cesarsouza at gmail.com
//
//    This library is free software; you can redistribute it and/or
//    modify it under the terms of the GNU Lesser General Public
//    License as published by the Free Software Foundation; either
//    version 2.1 of the License, or (at your option) any later version.
//
//    This library is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//    Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public
//    License along with this library; if not, write to the Free Software
//    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
//

namespace Accord.Statistics.Kernels
{
    using System;

    /// <summary>
    ///   Gaussian Kernel.
    /// </summary>
    /// 
    /// <remarks>
    /// <para>
    ///   The Gaussian kernel requires tuning for the proper value of σ. Different approaches
    ///   to this problem includes the use of brute force (i.e. using a grid-search algorithm)
    ///   or a gradient ascent optimization.</para>
    ///    
    /// <para>
    ///   References:
    ///   <list type="bullet">
    ///     <item><description>
    ///      P. F. Evangelista, M. J. Embrechts, and B. K. Szymanski. Some Properties of the
    ///      Gaussian Kernel for One Class Learning. Available on: http://www.cs.rpi.edu/~szymansk/papers/icann07.pdf </description></item>
    ///    </list></para>
    /// </remarks>
    /// 
    [Serializable]
    public sealed class Gaussian : IKernel, IDistance
    {
        private double sigma;
        private double gamma;

        /// <summary>
        ///   Constructs a new Gaussian Kernel
        /// </summary>
        /// 
        public Gaussian() : this(0) { }

        /// <summary>
        ///   Constructs a new Gaussian Kernel
        /// </summary>
        /// 
        /// <param name="sigma">The standard deviation for the Gaussian distribution.</param>
        /// 
        public Gaussian(double sigma)
        {
            this.Sigma = sigma;
        }

        /// <summary>
        ///   Gets or sets the sigma value for the kernel. When setting
        ///   sigma, gamma gets updated accordingly (gamma = 0.5*/sigma^2).
        /// </summary>
        /// 
        public double Sigma
        {
            get { return sigma; }
            set
            {
                sigma = value;
                gamma = 1.0 / (2.0 * sigma * sigma);
            }
        }

        /// <summary>
        ///   Gets or sets the gamma value for the kernel. When setting
        ///   gamma, sigma gets updated accordingly (gamma = 0.5*/sigma^2).
        /// </summary>
        /// 
        public double Gamma
        {
            get { return gamma; }
            set
            {
                gamma = value;
                sigma = System.Math.Sqrt(1.0 / (gamma * 2.0));
            }
        }

        /// <summary>
        ///   Gaussian Kernel function.
        /// </summary>
        /// 
        /// <param name="x">Vector <c>x</c> in input space.</param>
        /// <param name="y">Vector <c>y</c> in input space.</param>
        /// <returns>Dot product in feature (kernel) space.</returns>
        /// 
        public double Function(double[] x, double[] y)
        {
            // Optimization in case x and y are
            // exactly the same object reference.
            if (x == y) return 1.0;

            double norm = 0.0, d;
            for (int i = 0; i < x.Length; i++)
            {
                d = x[i] - y[i];
                norm += d * d;
            }

            return System.Math.Exp(norm * -gamma);
        }

        /// <summary>
        ///   Computes the distance in input space
        ///   between two points given in feature space.
        /// </summary>
        /// 
        /// <param name="x">Vector <c>x</c> in feature (kernel) space.</param>
        /// <param name="y">Vector <c>y</c> in feature (kernel) space.</param>
        /// <returns>Distance between <c>x</c> and <c>y</c> in input space.</returns>
        /// 
        public double Distance(double[] x, double[] y)
        {
            if (x == y) return 0.0;

            double norm = 0.0, d;
            for (int i = 0; i < x.Length; i++)
            {
                d = x[i] - y[i];
                norm += d * d;
            }

            // TODO: Verify the use of log1p instead
            return (1.0 / -gamma) * System.Math.Log(1.0 - 0.5 * norm);
        }

        /// <summary>
        ///   Computes the distance in input space given
        ///   a distance computed in feature space.
        /// </summary>
        /// 
        /// <param name="df">Distance in feature space.</param>
        /// <returns>Distance in input space.</returns>
        /// 
        public double Distance(double df)
        {
            return (1.0 / -gamma) * System.Math.Log(1.0 - 0.5 * df);
        }


    }
}
