namespace D11
{
    public class StaticStrings
    {

        public const string checkInternetUrl = "https://api.10cardsrummy.com/api/internetCheck";

        public const string AppName = "Demo";
        public const string AppId = "com.swipewire.RummyDemo";
        public const string RoundedAppIconResName = "Splash_13_app_icon_round";
        public const string AppIconResName = "";
        public const string AppSceneName = "";
        public const string PFAB_EmailTemplateIdKey = "AF4FE60B88C0C4B7";
        public const string ContactUsUrl = "http://rummyera.com/contact-us";

        public const string AvailChars = "abcdefghijklmnopqrstuvwxyz0123456789";
        public const string DateTimeFormat = "yyyyMMddHHmmssfff";
        public const string MatchEmailPattern =
        "^(?(\")(\".+?(?<!\\\\)\"@)|(([0-9a-z]((\\.(?!\\.))|[-!#\\$%&'\\*\\+/=\\?\\^`{}|~\\w])*)(?<=[0-9a-z])@))(?([)([(\\d{1,3}.){3}\\d{1,3}])|(([0-9a-z][-0-9a-z]*[0-9a-z]*.)+[a-z0-9][-a-z0-9]{0,22}[a-z0-9]))$";
        public const string MatchGmailPattern ="^[\\w.+\\-]+@gmail\\.com$";
        public const string PanCardPattern = "([A-Z]){5}([0-9]){4}([A-Z]){1}$";
        public const string AdhaarCardPattern = "^[2-9]{1}[0-9]{3}[0-9]{4}[0-9]{4}$";
        public const string Dob = @"(((19|20)\d\d)-(0[1-9]|1[0-2])-((0|1)[0-9]|2[0-9]|3[0-1]))$";
        public const string IFSCPattern = "^[A-Z]{4}0[A-Z0-9]{6}$";
#region SceneNames
        public const string LoadingScene = "Loading";
        public const string MainMenuScene = "MainMenu";
        public const string TutorialScene = "Tutorial";
#endregion

#region LeaderBoard
        public const int MaxBigContestCount = 11;
        public const int MaxSmallContestCount = 2;
        #endregion

        #region AWSCommands

        public static string Path = "cd /var/www/rummysquadsv2/api";

        public static string AddAmountToOpponentECSCommand = Path + " && sudo php artisan command:executeServerScript AddAmountToOpponent  ";
        public static string AddWinningAmountToOpponentECSCommand = Path + " && sudo php artisan command:executeServerScript  AddWinAmountFromOpponent  ";
        public static string SubractAmountFromOpponentECSCommand = Path + " && sudo php artisan command:executeServerScript  SubtractAmountFromOpponent  ";
        public static string UpdateStaticesECSCommand = Path + " && sudo php artisan command:executeServerScript  UpdateStaticsECS  ";
        public static string MatchLogECSCommand = Path + " && sudo php artisan command:executeServerScript  MatchLogs  ";
        public static string MatchCreateECSCommand = Path + " && sudo php artisan command:executeServerScript  MatchCreate  ";
        public static string MatchPlayersECSCommand = Path + " && sudo php artisan command:executeServerScript  MatchPlayer  ";
        public static string GetABotECSCommand = Path + " && sudo php artisan command:executeServerScript  GetABOT  ";
#endregion

#region AppKeys
        public const string PlayFab_SignUp_CustomLoginID = "CUSTOMIDFORSIGNUP";


        public const string PhotonAppID = "";
        public const string PhotonChatID = "";
        public const string PlayfabTitleID = "";
        public const string PlayfabSecretKey = "";
        #endregion

        #region PlayerPref Keys
        public const string Pref_LoginTypeKey = "LoginType";
        public const string Pref_SavedUserNameKey = "SavedUserName";
        public const string Pref_SavedPasswordKey = "SavedPassword";
        public const string Pref_LastGameRoomKey = "LastGameRoom";
        public const string Pref_LastGameLobbyKey = "LastGameLobby";
        public const string Pref_LastGamePlayerCountKey = "LastGamePlayerCount";
        public const string Pref_LastGameModeKey = "LastGameMode";
        public const string Pref_LastGameTableKey = "LastGameTable";
        public const string Pref_LastGameTypeKey = "LastGameType";
        public const string Pref_LastGameAmountTypeKey = "LastGameAmountType";
        public const string Pref_LastGameBootAmountKey = "LastGameBootAmount";
        public const string Pref_LastGameNameKey = "LastGameName";
        public const string Pref_LastGamePotAmountKey = "LastGamePotAmount";
        public const string Pref_LastGameIdKey = "LastGameId";
        public const string Pref_SoundToggleKey = "SoundToggle";
        public const string Pref_VibrateToggleKey = "VibrateToggle";
        public const string Pref_VolumeKey = "VolumeKey";
        public const string Pref_PortraitToggleKey = "PortraitToggle";
        public const string Pref_OrientationKey = "LandscapePortrait";

#endregion

#region Facebook
        public const string FB_AppID = "796739074307082";
        public const string FB_AppSecret = "eecf2ea36ffe04a788310cddb281a350";
        public const string FB_ShareLink = "";
        public const string FB_Sharelink_Title = "";
        public const string FB_Sharelink_Desc = "";
        public const string FB_Sharelink_Logo = "";
        public const string FB_FeedShareLink = "";
        public const string FB_FeedShareLink_Title = "";
        public const string FB_FeedShareLink_Caption = "";
        public const string FB_FeedShareLink_Desc = "";
        public const string FB_FeedShareLink_Picture = "";
        public const string FB_AppRequest_Msg = "";
        public const string FB_AppRequest_Title = "";
#endregion


#region ECS FunctionNames
        public const string ValidateReferralCodeECS = "ValidateReferralCodeECS";
        public const string ValidateMobileECS = "ValidateMobileECS";
        public const string ValidateMobileAndEmailECS = "ValidateMobileAndEmailECS";
        public const string ValidateOTPECS = "ValidateOTPECS";
        public const string GenerateOTPECS = "GenerateOTPECS";
        public const string UploadKYCDetailsECS = "UploadKYCDetailsECS";
        public const string UploadBankDetailsECS = "UploadBankDetailsECS";
        public const string ValidaPlayerOnLoginECS = "ValidaPlayerOnLoginECS";
        public const string AddAmountToUserECS = "AddAmountToUserECS";
        public const string SubtractAmountFromUserECS = "SubtractAmountFromUserECS";
        public const string SubtractAmountFromOpponentECS = "SubtractAmountFromOpponentECS";
        public const string AddWinningAmountECS = "AddWinningAmountECS";
        public const string AddWinningAmountOpponentECS = "AddWinningAmountOpponentECS";
        public const string UpdateStaticsECS = "UpdateStaticsECS";
        public const string ValidateDailyBonusRewardsECS = "PlayersDailyClaimECS";
        public const string UpdateRewardStaticsECS = "UpdateRewardStaticsECS";
        public const string UpdateTournamentResultECS = "UpdateTournamentResultECS";
        public const string UpdateBonusECS = "UpdateBonusECS";
        public const string MatchCreateECS = "MatchCreateECS";
        public const string MatchPlayersECS = "MatchPlayersECS";
        public const string MatchLogsECS = "MatchLogsECS";
        public const string SendFriendRequestECS = "SendFriendRequest";
        public const string InitPlayerDataECS = "InitPlayerData";
        public const string AcceptFriendRequestECS = "AcceptFriendRequest";
        public const string RejectFriendRequestECS = "DenyFriendRequest";
        public const string AddSpinamountECS = "AddSpinamountECS";
       // public const string AddAmountToOpponentECS = "AddAmountToOpponentECS";
#endregion

#region Error Codes and Messages

        public const string Error_Empty = " cannot be empty";
        public const string Error_Email_Password_Invalid = "Email Id or Password is not Valid";
        public const string Error_Email_Invalid = "Email ID is not valid";
        public const int Password_Char_Length = 6;
        public const string Error_Password_Invalid_Len = "Password must be between 6 - 12 characters";
        public const int NickName_Char_Length = 6;
        public const string Error_NickName_Invalid_Len = "Username should be between 6-12 characters";
        public const int Mobile_Char_Length = 10;
        public const string Error_Mobile_Invalid_Len = "Mobile Number must be 10 digits";
        public const string Error_Mobile_Invalid = "Enter valid mobile";
        public const string Error_Mobile_Not_Avail = "Mobile Number not available";
        public const int Referral_Char_Length = 6;
        public const string Error_Referral_Invalid_Len = "Referral code must be min XXXX character";
        public const string Error_Referral_Invalid = "Referral code invalid";
        public const string Error_Facebook_ID_Invalid = "Login with facebook again";
        public const string Error_OTP_Invalid = "Invalid OTP!";
        public const string Error_OTP_Expired = "OTP Expired!";
        public const string Error_Image_Capture_Failed = "Image capturing failed";
        public const string Error_ScreenShot_Share = "Couldn't share screenshot now, please try again later!";
        public const string Error_Agree_Terms_Select = "Please accepts terms and conditions";
        public const string Error_Playfab_Generic = "Something wrong, please try again later!";
        public const string Error_Internet = "Please Check your Internet Connection";
        public const string Success_Login = "Logged in successfully";
        public const string Forgot_Password = "Password recovery mail sent";
        public const string Success_Send_OTP = "OTP sent successfully!";
        public const string Success_Verified_OTP = "OTP verified successfully!";
        public const string Success_Referral_Verified = "Referral code added successfully!\n Bonus amount of XXXX claimed.";
        public const string Success_Details_Uploaded = "Details submited successfully";
        public const string Success_DailyRewards_Claimed = "Daily Rewards claimed successfully!";
        public const string KYC_DOB_Invalid = "<color=#FF0000>DOB must be in yyyy-mm-dd</color>";
        public const string KYC_PanID_Invalid = "<color=#FF0000>Enter valid Pan number</color>";
        public const string KYC_Aadhaar_Invalid = "<color=#FF0000>Enter valid aadhaar number</color>";
        public const string KYC_Details_Required = "KYC details required to proceed further";
        public const string KYC_Details_Must_Approved = "KYC details must get approved to proceed further";
        public const string Bank_Details_Required = "Bank details required to proceed further";
        public const string Bank_details_Must_Approved = "Bank details must get approved to proceed further";
        public const string Bank_IFSC_Invalid = "Enter valid IFSC code";
        public const string Payment_Added_Success = "Payment Success!";
        public const string Payment_Added_Failed = "Payment failed!";
        public const string Payment_Validate_Retry = "Payment success but couldn't verify due to timeout. Click Ok to retry.";
        public const string Payment_Added_Info = "Rs.XXX deposited successfully";
        public const string Payment_Back_Error = "User cancelled payment";
        public const string WithDraw_Request_Success = "Withdraw amount request sent successfully";
        public const string WithDraw_LowBalance_Error = "Low balance";
        public const string WithDraw_MinAmount_Error = "Withdraw amount can't be less than min amount";
        public const string WithDraw_MaxAmount_Error = "Withdraw amount can't be greater than max amount";


#endregion

#region PlayFab Keys
        public const string PlayfabIDKey = "PlayfabID";
        public const string NickNameKey = "NickName";
        public const string FullNameKey = "FullName";
        public const string FacebookUserId = "FacebookUserId";
        public const string AvatarIndexKey = "AvatarIndex";
        public const string AvatarUrlKey = "AvatarUrl";        
        public const string MobileNumberKey = "MobileNo";
        public const string EmailIDKey = "EmailId";      
        public const string ReferralCodeKey = "ReferralCode";
        public const string ReferralCountKey = "ReferralCount";
        public const string ReferredByKey = "ReferredBy";
        public const string DailyRewardsKey = "DailyRewards";
        public const string DateofBirthKey = "DateOfBirth";
        public const string WatchedVideoCountKey = "WatchedVideoCount";
        public const string DeviceIDKey = "DeviceID";
        public const string BlockStatusKey = "BlockStatus";
        public const string CreatedDateKey = "CreatedDate";
        public const string LatestPaymentStatusKey = "LatestPaymentStatus";
        public const string DepositPlayerTokenKey = "DepositPlayerToken";
        public const string TournamentDetailsKey = "TournamentDetails";
        public const string ExtraBonusKey = "ExtraBonus";
        public const string IsServerUnderMaintenanceKey = "IsServerUnderMaintenance";
        public const string PlayerLevelKey = "PlayerLevel";
        public const string InitialDepositKey = "InitialDeposit";
        public const string GamesPlayedKey = "GamesPlayed";
        public const string GamesWonKey = "GamesWon";
        public const string NetWinnigsKey = "NetWinning";
        public const string NetLostKey = "NetLost";
        public const string TrophyValKey = "TrophyVal";
        public const string BonusWinnigsKey = "BonusWinning";
        public const string LockBonusKey = "LockBonus";
        public const string TotalDepositKey = "TotalDeposit";
        public const string TotalWithDrawKey = "TotalWithDraw";
        public const string PlayerXPKey = "PlayerXP";
        public const string RewardedVideoCountKey = "RewardedVideoCount";
        public const string WeeklyHandsWonKey = "WeeklyHandsWon";
        public const string SpinWheelLastClaimedKey = "SpinWheelLastClaimedKey";
        public const string GoldDepositVal = "GoldDepositVal";
        public const string GoldWinningVal = "GoldWinningVal";
        public const string GoldBonusVal = "GoldBonusVal";
        public const string GoldVal = "GoldVal";
        public const string SilverValKey = "SilverVal";
        public const string AddAmountToOpponentECS = "AddAmountToOpponentECS";

        public const string ChatsKey = "Chats";
        public const string EmojiKey = "Emoji";

        public const string BankDetailsKey = "BankDetails";
        public const string KYCDetailsKey = "KYCDetails";
        public const string KYCStatusKey = "KYCStatus";

        public const string AccountNameKey = "AccountName";
        public const string AccountNumberKey = "AccountNo";
        public const string BankBranchKey = "BankBranch";
        public const string BankNameKey = "BankName";
        public const string IFSCCodeKey = "IFSCCode";
        public const string AccountName2Key = "AccountName2";
        public const string AccountNumber2Key = "AccountNo2";
        public const string BankName2Key = "BankName2";
        public const string BankBranch2Key = "BankBranch2";
        public const string IFSCCode2Key = "IFSCCode2";

        public const string AadhaarNoKey = "AadhaarNo";
        public const string AadhaarUrlKey = "AadharCardPicUrl";
        public const string PANNoKey = "PANNo";
        public const string PANCardUrlKey = "PANCardPicUrl";

        public const string PaymentCollectUrlKey = "PaymentCollectUrl";
        public const string PaymentValidateUrlKey = "PaymentValidateUrl";
        public const string PaymentTransactionUrlKey = "PaymentTransactionUrl";
        public const string PaymentWithdrawUrlKey = "PaymentWithdrawUrl";
        public const string TermsUrlKey = "TermsUrl";
        public const string RefundPolicyKey = "RefundPolicy";
        public const string ContactUsKey = "ContactUs";
        public const string PrivacyPolicyUrlKey = "PrivacyPolicyUrl";
        public const string FileUploadUrlKey = "FileUploadUrl";
        public const string GetOtpUrlKey = "GetOtpUrl";
        public const string ValidateOtpUrlKey = "ValidateOtpUrl";
        public const string ValidateMobileNoUrlKey = "ValidateMobileNoUrl";

        public const string PFAB_AccountInfoKey = "AccountInfo";
        public const string PFAB_PlayerProfileKey = "PlayerProfile";
        public const string PFAB_TitleDataKey = "TitleData";
        public const string PFAB_UserDataKey = "UserData";
        public const string PFAB_ReadOnlyUserDataKey = "UserReadOnlyData";
        public const string PFAB_StatisticsDataKey = "PlayerStatistics";

        public const string TD_AppVersionKey = "AppVersion";
        public const string TD_AndroidAppURLKey = "AndroidAppUrl";
        public const string TD_SupportEmailKey = "SupportEmail";
        public const string TD_WebsiteUrlKey = "WebsiteUrl";
        public const string TD_GameTableKey = "GameTables";
        public const string TD_DailyBonusKey = "DailyRewards";
        public const string TD_ReferalBonusKey = "ReferralBonus";
        public const string TD_SignUpBonusKey = "SignUpBonus";
        public const string TD_PaymentUrlsKey = "PaymentUrls";
        public const string TD_HotTableKey = "Hotgames";
        public const string TD_ShareMsgTemplateKey = "ShareMsgTemplate";
        public const string TD_PrivateRoomMsgTemplateKey = "PrivateRoomMsgTemplate";
        public const string TD_RefferalMsgTemplateKey = "RefferalMsgTemplate";
        public const string TD_RoomSummaryMsgTemplateKey = "RoomSummaryMsgTemplate";
        public const string TD_BankDetailsTemplateKey = "BankDetailsTemplate";
        public const string TD_KYCDetailsTemplateKey = "KYCDetailsTemplate";
        public const string TD_MaxPingKey = "MaxPing";
        public const string TD_CommissionPercentageKey = "WinCommissionPercentage";
        public const string TD_CopyrightsKey = "CopyRights";
        public const string TD_SignUpBonusGoldDeposit = "SignUpBonusGoldDeposit";
        public const string TD_FirstGoldDepositBonusPercentage = "FirstGoldDepositBonusPercentage";
        public const string TD_WelcomeTextKey = "WelcomeText";
        public const string PlayerVerifiedKey = "PlayerVerified";
#endregion


#region RoomProperties Keys
        public const string ROOM_GAME_STATE = "GameState";
        public const string ROOM_GAME_COINTYPE = "CT";
        public const string ROOM_GAME_GAMEMODE = "GM";
        public const string ROOM_GAME_GAMETABLE = "GT";
#endregion

#region InfoTable Value
        public const string NOLimit = "UNLIMITED";
        public const string NormalWin = "4";
        public const string BigWin = "50";
#endregion InfoTable Value



#region D11Ingame

        public const string Elimination = "You have been eliminated from the game.";
        public const string EliminationPreviousGame = "You have been removed from the previous game.";
        public const string WinnerMsg = "Congratulations! You are the winner of this game. Your account will be credited with xxx shortly.";
        public const string TossNoti = "xxx Won the toss. Wait till yyy sec.";
        public const string OpponentDeclare = "xxx declare the card wait for yyy sec.";
        public const string DeclareNoti = " has shown their card. "+ArangeCardNoti;
        public const string ArangeCardNoti = "Please arrange your cards into valid sequences or sets.";
        public const string WaitOpponentDeclareNoti = "Waiting for opponent to declare. xxx Sec";
        public const string InsufficientBalance = "You need a minimum of xxx to play this game.";
        public const string Pick_a_card = "Your turn to pick a card";
        public const string Discard_a_card = "Please discard a card";
        public const string ExitErrorMsg = "Unfortunately you cannot exit the table as your buy-in amount has been locked for this round.";
        public const string DragCanvasName = "DragCanvas";
        public const string InternetCheck = "Your internet connection is too poor.";
        public const string InternetWarning = "Check your internet connection.";
        public const string Reconnectalert = "Click here for Reconnection to server.";
        public const string WaitingForPlayers = "Waiting for players to join";
        public const string NetworkIssue = "Your internet is highly unstable. Connect to a better internet for participating.";
#endregion

#region D11

   
        #endregion


        #region URLs
       

        #endregion

        #region URL Keys
        public const string URL_EmailMobile_Key = "EmailMobile";
        public const string URL_IsMobileOtp_Key = "IsMobileOtp";
        public const string URL_IsResendOtp_Key = "IsResend";
        public const string URL_OtpVal_Key = "OtpVal";
        public const string URL_FileValue_Key = "filename";
        public const string URL_PicType_Key = "type";
        public const string URL_PlayfabId_Key = "playerPlayfabId";
        public const string URL_ProfilePic_Key = "PROFILE_PIC";
        public const string URL_BankDetails_Key = "BankDetails";
        public const string URL_Amount_Key = "amount";
        public const string URL_Phone_Key = "phone";
        public const string URL_Email_Key = "email";
        public const string URL_IsGame_Key = "isGame";

        public const string URL_CashType_Key = "cash_type";

        public const string URL_GameName_Key = "gameName";
        public const string URL_UniqueId_Key = "uniqueId";
        public const string URL_paymentToken_Key = "paymentToken";
        public const string URL_ValidatePayment_Key = "pId";
        //public const string URL_CashType_Key = "cashtype";

#endregion

#region BOT_Name

#endregion BOT_Name

    }


}