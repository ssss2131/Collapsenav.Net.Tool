using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Xunit;

namespace Collapsenav.Net.Tool.Test;
public class UserInfo
{
    public string UserName { get; set; }
    public int Age { get; set; }
}
public class UserInfoTwo
{
    public string UserName { get; set; }
    public int Age { get; set; }
}
public class JsonTest
{
    [Fact]
    public void ToObjCollectionTest()
    {
        string userInfo = @"
            [
                {
                    ""userName"": ""ABCD"",
                    ""age"": 23
                }
            ]";
        IEnumerable<UserInfo> user = userInfo.ToObjCollection<UserInfo>();
        Assert.True(user.First().UserName == "ABCD" && user.First().Age == 23);
    }


    [Fact]
    public void ToObjTest()
    {
        string userInfo = @"
            {
                ""userName"": ""ABCD"",
                ""age"": 23
            }";
        var user = userInfo.ToObj<UserInfo>();
        Assert.True(user.UserName == "ABCD" && user.Age == 23);
        user = userInfo.ToObj(typeof(UserInfo)) as UserInfo;
        Assert.True(user.UserName == "ABCD" && user.Age == 23);
    }

    [Fact]
    public void ToJsonTest()
    {
        UserInfo user = new()
        {
            UserName = "ABCD",
            Age = 23
        };
        var jsonString = user.ToJson().Trim();
        Assert.True("{\"userName\":\"ABCD\",\"age\":23}" == jsonString);
    }
    [Fact]
    public void JsonMapTest()
    {
        UserInfo user = new()
        {
            UserName = "ABCD",
            Age = 23
        };
        var user2 = user.JsonMap<UserInfoTwo>();
        Assert.True(user.UserName == user2.UserName && user.Age == user2.Age);

        List<UserInfo> users = new()
        {
            new UserInfo { UserName = "1", Age = 1 },
            new UserInfo { UserName = "2", Age = 2 },
            new UserInfo { UserName = "3", Age = 3 },
            new UserInfo { UserName = "4", Age = 4 },
        };
        var users2 = users.JsonMap<IEnumerable<UserInfoTwo>>();
        Assert.True(users.First().UserName == users2.First().UserName && users.First().Age == users2.First().Age);
    }

    [Fact]
    public void JsonMapNoObjectTest()
    {
        UserInfo user = new()
        {
            UserName = "ABCD",
            Age = 23
        };
        var user2 = user.JsonMap<UserInfo, UserInfoTwo>();
        Assert.True(user.UserName == user2.UserName && user.Age == user2.Age);

        List<UserInfo> users = new()
        {
            new UserInfo { UserName = "1", Age = 1 },
            new UserInfo { UserName = "2", Age = 2 },
            new UserInfo { UserName = "3", Age = 3 },
            new UserInfo { UserName = "4", Age = 4 },
        };
        var users2 = users.JsonMap<IEnumerable<UserInfo>, IEnumerable<UserInfoTwo>>();
        Assert.True(users.First().UserName == users2.First().UserName && users.First().Age == users2.First().Age);
    }

    [Fact]
    public void JsonMapFromObjToObjTest()
    {
        var obj1 = new TestJsonObj1 { Age = 1, UserName = "1", Id = 1 };
        var obj2 = new TestJsonObj2 { Age = 2, UserName = "2" };
        var temp = obj1.JsonMap(obj2);
        Assert.True(temp.Age == 1);
        Assert.True(temp.UserName == "1");
        var temp2 = obj2.JsonMap(obj1);
        Assert.True(temp2.Age == 2);
        Assert.True(temp2.UserName == "2");
        Assert.True(temp2.Id == 1);
    }

    [Fact]
    public void JsonNodeToObjTest()
    {
        var obj1 = new TestJsonObj1 { Age = 1, UserName = "1", Id = 1 };
        var objStr = obj1.ToJson();
        var sd = objStr.ToJsonNode().ToObj<TestJsonObj1>();
        Assert.True(sd.Age == 1);
        Assert.True(sd.UserName == "1");
        Assert.True(sd.Id == 1);
    }

    [Fact]
    public void DateConverterTest()
    {
        var obj = new DateConverterTestModel
        {
            Date = new DateTime(2022, 12, 12, 12, 12, 12)
        };
        var jsonString = obj.ToJson();
        Assert.True(jsonString.Contains("2022-12-12"));
        obj = jsonString.ToObj<DateConverterTestModel>();
        Assert.Equal(obj.Date.Year, 2022);
        Assert.Equal(obj.Date.Month, 12);
        Assert.Equal(obj.Date.Day, 12);
        Assert.Equal(obj.Date.Hour, 0);
        Assert.Equal(obj.Date.Minute, 0);
        Assert.Equal(obj.Date.Second, 0);
    }
    [Fact]
    public void DateTimeConverterTest()
    {
        var obj = new DateTimeConverterTestModel
        {
            Date = new DateTime(2022, 12, 12, 12, 12, 12)
        };
        var jsonString = obj.ToJson();
        Assert.True(jsonString.Contains("2022-12-12 12:12:12"));
        obj = jsonString.ToObj<DateTimeConverterTestModel>();
        Assert.Equal(obj.Date.Year, 2022);
        Assert.Equal(obj.Date.Month, 12);
        Assert.Equal(obj.Date.Day, 12);
        Assert.Equal(obj.Date.Hour, 12);
        Assert.Equal(obj.Date.Minute, 12);
        Assert.Equal(obj.Date.Second, 12);
    }
    [Fact]
    public void DateMilliTimeConverterTest()
    {
        var obj = new DateMilliTimeConverterTestModel
        {
            Date = new DateTime(2022, 12, 12, 12, 12, 12, 233)
        };
        var jsonString = obj.ToJson();
        Assert.True(jsonString.Contains("2022-12-12 12:12:12.233"));
        obj = jsonString.ToObj<DateMilliTimeConverterTestModel>();
        Assert.Equal(obj.Date.Year, 2022);
        Assert.Equal(obj.Date.Month, 12);
        Assert.Equal(obj.Date.Day, 12);
        Assert.Equal(obj.Date.Hour, 12);
        Assert.Equal(obj.Date.Minute, 12);
        Assert.Equal(obj.Date.Second, 12);
        Assert.Equal(obj.Date.Millisecond, 233);
    }

    [Fact]
    public void SnowflakeConverterTest()
    {
        var id = SnowFlake.NextId();
        var obj = new SnowflakeConverterTestModel
        {
            Id = id
        };
        var jsonString = obj.ToJson();
        Assert.True(jsonString.Contains($"\"{obj.Id}\""));
        obj = jsonString.ToObj<SnowflakeConverterTestModel>();
        Assert.True(obj.Id == id);
    }

    public class DateConverterTestModel
    {
        [JsonConverter(typeof(DateConverter))]
        public DateTime Date { get; set; }
    }

    public class DateTimeConverterTestModel
    {
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Date { get; set; }
    }
    public class DateMilliTimeConverterTestModel
    {
        [JsonConverter(typeof(DateMilliTimeConverter))]
        public DateTime Date { get; set; }
    }

    public class SnowflakeConverterTestModel
    {
        [JsonConverter(typeof(SnowflakeConverter))]
        public long Id { get; set; }
    }

    public class TestJsonObj1
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int Age { get; set; }
    }

    public class TestJsonObj2
    {
        public string UserName { get; set; }
        public int Age { get; set; }
    }
}

