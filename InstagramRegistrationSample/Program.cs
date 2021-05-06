/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ My Telegram Account: https://t.me/ramtinak ]
 * 
 * 
 * This sample is a part of InstagramApiSharp's private library.
 * 
 * 
 * IRANIAN DEVELOPERS
 */

using InstagramApiSharp;
using InstagramApiSharp.API;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.SessionHandlers;
using InstagramApiSharp.Enums;
using InstagramApiSharp.Logger;
using System;
using System.Threading.Tasks;

/*
 * ////////////////////////////// WARNING /////////////////////////////////
 * 
 * 1. You need InstagramApiSharp v1.6.0 for running this sample!
 * 2. Registration feature only works on .NETFramework 4.6.1 or newer or .NETStandard 2.0 or newer or .NETCore.
 * 3. Note that I didn't implement v180 API in the InstagramApiSharp's public library,
 *    so it's only works for registration!!!!! You have to modify it a little bit by yourself 
 *    to support it for LoginAsync and other stuff!
 * 4. Registering account via Phone is just like this one, so I didn't add it! 
 *    but if you need it, you can purchase the library, it's already has the sample for phone registration!
 * 
 * ////////////////////////////// WARNING /////////////////////////////////
 * 
 */
/*
 * //////////////////////////////////////////////// NOTE ////////////////////////////////////////////////
 * 
 * If you want to reference the InstagramApiSharp's project directly, you must add following library from nuget:
 * 
 * 1. Microsoft.CSharp          version 4.3.0 or newer
 * 
 * 2. Portable.BouncyCastle     version 1.8.6.7 or newer
 * 
 * 3. Newtonsoft.Json           version 10.0.3 or newer
 * 
 * //////////////////////////////////////////////// NOTE ////////////////////////////////////////////////
 * 
 */

namespace InstagramRegistrationSample
{
    /*
     * 
     * DO NOT ASK ANY QUESTION! I WON'T ANSWER TO REGISTRATION QUESTIONS! SO DON'T BOTHER YOURSELF!!!!
     * 
     */
    class Program
    {
        private static IInstaApi InstaApi;

#pragma warning disable IDE0060 // Remove unused parameter
        static void Main(string[] args)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            Task.Run(MainAsync).GetAwaiter().GetResult();
            Console.ReadKey();
        }

        static async Task MainAsync()
        {
            var username = "username";
            var password = "password";
            var email = "abc@email.com";
            var firstName = "my name"; // optional, but don't pass null, put string.Empty or ""


            var userSession = new UserSessionData
            {
                UserName = username,
                Password = password
            };

            var delay = RequestDelay.FromSeconds(2, 4);
            InstaApi = InstaApiBuilder.CreateBuilder()
                .SetUser(userSession)
                .UseLogger(new DebugLogger(LogLevel.All))
                .SetRequestDelay(delay)
                // session handler, set a file path to save/load your state/session data
                .SetSessionHandler(new FileSessionHandler { FilePath = username + ".state" })
                .Build();

            // Set api version to v180 [ PLEASE READ THE WARNING SECTION IN THIS PAGE! ]
            InstaApi.SetApiVersion(InstaApiVersionType.Version180);

            // no need to do this but I just wanted to show you
            InstaApi.RegistrationService.Birthday = InstaApi.RegistrationService.GenerateRandomBirthday();


            email = email.ToLower();
            username = username.ToLower();

            await InstaApi.RegistrationService.FirstLauncherSyncAsync();
            await InstaApi.RegistrationService.FirstQeSyncAsync();

            var checkEmailResult = await InstaApi.RegistrationService.CheckEmailAsync(email);
            // if email address is available to create
            if (checkEmailResult.Succeeded && (checkEmailResult.Value?.Available ?? false))
            {
                await Delay(1.5);
                var signupConsent = await InstaApi.RegistrationService.GetSignupConsentConfigAsync();
                await Delay(1.5);
                await InstaApi.RegistrationService.SendRegistrationVerifyEmailAsync(email);

                // check registration code that instagram sent it to your email:
                var verificationCode = "";
                await Delay(3.5);
                var checkRegistrationConfirmationResult = await InstaApi.RegistrationService
                    .CheckRegistrationConfirmationCodeAsync(email, verificationCode);

                if (checkRegistrationConfirmationResult.Succeeded)
                {

                    await InstaApi.RegistrationService.GetSiFetchHeadersAsync();

                    await Delay(1.5);
                    await InstaApi.RegistrationService.GetUsernameSuggestionsAsync(firstName, email);

                    await Delay(1.5);

                    if (signupConsent.Value?.AgeRequired ?? false)
                        await InstaApi.RegistrationService.CheckAgeEligibilityAsync();

                    await Delay(2.5);
                    await InstaApi.RegistrationService.GetOnboardingStepsAsync(InstaOnboardingProgressState.Prefetch);

                    await InstaApi.RegistrationService.NewUserFlowBeginsConsentAsync();
                    await InstaApi.RegistrationService.NewUserFlowBeginsConsentAsync();

                    await Delay(2.5);
                    var checkUsernameResult = await InstaApi.RegistrationService.CheckUsernameAsync(username);
                    if (checkUsernameResult.Succeeded && (checkUsernameResult.Value?.Available ?? false))
                    {
                        var signupCode = checkRegistrationConfirmationResult.Value.SignupCode;
                        var createAccount =
                            await InstaApi.RegistrationService.CreateNewAccountWithEmailAsync(email, username, password, firstName, signupCode);
                        if (!createAccount.Succeeded)
                        {
                            Console.WriteLine(createAccount.Info.Message);
                            return;
                        }

                        await InstaApi.RegistrationService.GetMultipleAccountsFamilyAsync();
                        await InstaApi.RegistrationService.GetZrTokenResultAsync();
                        await InstaApi.RegistrationService.LauncherSyncAsync();
                        await InstaApi.RegistrationService.QeSyncAsync();
                        await InstaApi.RegistrationService.NuxNewAccountSeenAsync();
                        await InstaApi.RegistrationService.GetOnboardingStepsAsync(InstaOnboardingProgressState.Start);
                        await InstaApi.RegistrationService.GetContactPointPrefillAsync();
                        await InstaApi.RegistrationService.GetOnboardingStepsAsync(InstaOnboardingProgressState.Finish);


                        // now we can save our session to local:
                        InstaApi.SessionHandler?.Save();

                    }
                }
            }
        }

        static async Task Delay(double seconds)
        {
            await Task.Delay(TimeSpan.FromSeconds(seconds));
        }
    }
}
