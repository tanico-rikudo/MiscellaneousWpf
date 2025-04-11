using Microsoft.Extensions.Configuration;
using System;

namespace LiveChartPlay.Helpers
{
    public static class ConfigurationBuilderHelper
    {
        public static IConfiguration BuildConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory) // アプリの実行ディレクトリを設定
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // JSONを読み込む
                .Build(); // IConfiguration オブジェクトを返す
        }
    }
}
