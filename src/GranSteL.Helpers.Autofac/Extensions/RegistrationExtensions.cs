﻿// This software is part of the Autofac IoC container
// Copyright © 2011 Autofac Contributors
// https://autofac.org
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.ServiceModel;
using Autofac;
using Autofac.Builder;

namespace GranSteL.Helpers.Autofac.Extensions
{
    /// <summary>
    /// Extend the registration syntax with WCF-specific helpers.
    /// </summary>
    public static class RegistrationExtensions
    {
        /// <summary>
        /// Dispose the channel instance in such a way that exceptions aren't thrown
        /// if a faulted channel is closed.
        /// </summary>
        /// <typeparam name="TLimit">Registration limit type.</typeparam>
        /// <typeparam name="TActivatorData">Activator data type.</typeparam>
        /// <typeparam name="TRegistrationStyle">Registration style.</typeparam>
        /// <param name="registration">Registration to set release action for.</param>
        /// <returns>Registration builder allowing the registration to be configured.</returns>
        /// <remarks>This will eat exceptions generated in the closing of the channel.</remarks>
        public static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle>
            UseWcfSafeRelease<TLimit, TActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> registration)
        {
            // When a channel is closed in WCF, the Dispose method calls Close internally.
            // If the channel is in a faulted state, the Close method will throw, yielding
            // an incorrect exception to be thrown during disposal. This extension fixes
            // that design problem.
            if (registration == null) throw new ArgumentNullException(nameof(registration));
            return registration.OnRelease(CloseChannel);
        }

        static void CloseChannel<T>(T channel)
        {
            var communicationObject = (ICommunicationObject)channel;
            try
            {
                if (communicationObject.State == CommunicationState.Faulted)
                    communicationObject.Abort();
                else
                    communicationObject.Close();
            }
            catch (TimeoutException)
            {
                communicationObject.Abort();
            }
            catch (CommunicationException)
            {
                communicationObject.Abort();
            }
            catch (Exception)
            {
                communicationObject.Abort();
                throw;
            }
        }
    }
}