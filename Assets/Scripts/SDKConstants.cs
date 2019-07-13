using UnityEngine;

public class SDKConstants
{
    // Error Code
    public const int SUCCESS = 0;	                //成功
    public const int ERROR_CANCEL = -1;	            //取消
    public const int ERROR_FAIL = -2;	            //失败
    public const int ERROR_FAIL_DETECT = -3;	    //检测失败
    // Login
    public const int ERROR_LOGIN_CANCEL = -1;	    //登录取消
    public const int ERROR_LOGIN_FAIL = -2;	        //登录失败
    // Pay
    public const int ERROR_PAY_CANCEL = -1;	        //支付取消
    public const int ERROR_PAY_FAIL = -2;	        //支付失败
    public const int ERROR_PAY_ORDER_SUBMIT = -3;   // 充值卡订单已经提交
    // 
    public const int ERROR_QUIT_GAME = 3;           // 退出游戏(弹渠道的退出框,不弹出游戏退出框)
    public const int ERROR_QUIT_GAME_0 = 4;         // 退出游戏(弹出游戏退出框)
    public const int ERROR_CONTINUE_GAME = 5;       // 继续游戏

    public const int ERROR_SIGNOFF = 6;             // 注销

    // Method Name
    public static readonly string Method_Login = "login";

}
