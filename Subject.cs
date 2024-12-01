using System;
using System.Collections.Generic;

public interface ISubject
{
    string Request(string request);
}

public class RealSubject : ISubject
{
    public string Request(string request)
    {
        // Логика, которую мы хотим защитить или контролировать
        return $"RealSubject: обработан запрос '{request}'";
    }
}

public class Proxy : ISubject
{
    private readonly RealSubject _realSubject;
    private readonly Dictionary<string, (string response, DateTime timestamp)> _cache;
    private readonly TimeSpan _cacheDuration;
    private readonly string _userRole;

    public Proxy(string userRole)
    {
        _realSubject = new RealSubject();
        _cache = new Dictionary<string, (string, DateTime)>();
        _cacheDuration = TimeSpan.FromMinutes(5); // Время кэширования
        _userRole = userRole; // Роль пользователя
    }

    public string Request(string request)
    {
        // Проверка прав доступа
        if (_userRole != "admin")
        {
            return "Доступ запрещен. У вас нет прав для выполнения этого запроса.";
        }

        // Проверка кэша
        if (_cache.TryGetValue(request, out var cachedResponse))
        {
            if (DateTime.Now - cachedResponse.timestamp < _cacheDuration)
            {
                return cachedResponse.response; // Возвращаем кэшированный ответ
            }
            else
            {
                _cache.Remove(request); // Удаляем устаревший кэш
            }
        }

        // Если нет кэша, вызываем RealSubject
        var response = _realSubject.Request(request);

        // Сохраняем результат в кэше
        _cache[request] = (response, DateTime.Now);

        return response;
    }
}
