using System.Collections.Generic; // 引入集合命名空间
using System.IO; // 引入输入输出命名空间，用于文件操作
using Newtonsoft.Json; // 引入Json.NET库，用于JSON序列化和反序列化
using System.Linq; // 引入LINQ，用于简化集合操作
using TMPro; // 引入TextMeshPro命名空间，用于高质量文本显示
using UnityEngine; // 引入Unity引擎核心命名空间
using UnityEngine.UI; // 引入UI命名空间，用于操作UI元素

public class LitUserUserSystem : MonoBehaviour // 主类定义
{
    private string jsonFilePath; // 存储用户数据文件的路径
    private List<UserData> usernames; // 存储用户数据的列表
    public TMP_InputField usernameInput; // 用户名输入字段
    public Button registerButton; // 注册按钮
    public Button loadButton; // 加载按钮
    public Button clearButton; // 清除按钮
    public TMP_Dropdown userroledropdown; // 用户角色下拉菜单

    private void Start() // Unity的Start方法，在对象启动时调用
    {
        // 设置jsonFilePath为persistentDataPath下的文件路径
        jsonFilePath = Path.Combine(Application.persistentDataPath, "LitUserSystem.json");

        usernames = new List<UserData>(); // 初始化用户数据列表
        registerButton.onClick.AddListener(RegisterUsername); // 为注册按钮添加事件监听器
        clearButton.onClick.AddListener(ClearJsonContent); // 为清除按钮添加事件监听器
        loadButton.onClick.AddListener(OnLoadButtonClick); // 为加载按钮添加事件监听器
    }

    public void RegisterUsername() // 注册用户名方法
    {
        string username = usernameInput.text; // 获取输入的用户名
        string profession = userroledropdown.options[userroledropdown.value].text; // 获取选中的职业

        if (string.IsNullOrWhiteSpace(username)) // 检查用户名是否为空
        {
            Debug.Log("Username cannot be empty."); // 打印日志
            return;
        }

        if (usernames.Any(user => user.Username == username)) // 检查用户名是否已存在
        {
            Debug.Log("Username already exists."); // 打印日志
            return;
        }

        usernames.Add(new UserData { Username = username, Profession = profession }); // 添加新用户数据
        SaveUsernames(); // 保存用户数据到文件
        Debug.Log("Username registered successfully."); // 打印日志
    }

    public void OnLoadButtonClick() // 加载按钮点击事件处理方法
    {
        string username = usernameInput.text; // 获取输入的用户名
        List<UserData> users = LoadUserData(); // 加载用户数据

        UserData user = users.FirstOrDefault(u => u.Username == username); // 查找用户
        if (user != null) // 如果找到用户
        {
            userroledropdown.captionText.text = user.Profession; // 设置下拉菜单显示的职业
            userroledropdown.interactable = false; // 禁用下拉菜单
        }
        else
        {
            Debug.Log("Username not found."); // 打印日志
        }
    }

    private List<UserData> LoadUserData() // 加载用户数据方法
    {
        if (File.Exists(jsonFilePath)) // 检查文件是否存在
        {
            string jsonContent = File.ReadAllText(jsonFilePath); // 读取文件内容
            return JsonConvert.DeserializeObject<List<UserData>>(jsonContent) ?? new List<UserData>(); // 反序列化JSON内容
        }
        return new List<UserData>(); // 如果文件不存在，返回空列表
    }

    private void SaveUsernames() // 保存用户数据方法
    {
        string json = JsonConvert.SerializeObject(usernames); // 序列化用户数据为JSON字符串
        File.WriteAllText(jsonFilePath, json); // 将JSON字符串写入文件
    }

    private void ClearJsonContent() // 清除JSON文件内容方法
    {
        usernames.Clear(); // 清空用户数据列表
        File.WriteAllText(jsonFilePath, JsonConvert.SerializeObject(new List<string>())); // 将空列表序列化并写入文件
        Debug.Log("JSON content cleared."); // 打印日志
        usernameInput.text = string.Empty; // 清空用户名输入字段
        userroledropdown.interactable = true; // 启用下拉菜单
    }
}

public class UserData // 用户数据类
{
    public string Username { get; set; } // 用户名属性
    public string Profession { get; set; } // 职业属性
}