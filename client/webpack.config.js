const path = require('path');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const webpack = require('webpack');

const paths = require('./paths');

const hasJsxRuntime = (() => {
   try {
      require.resolve('react/jsx-runtime');
      return true;
   } catch (e) {
      return false;
   }
})();

const CSSLoader = {
   test: /\.css$/,
   use: [
      MiniCssExtractPlugin.loader,
      'css-loader',
      {
         loader: 'postcss-loader',
         options: {
            postcssOptions: {
               config: path.resolve(__dirname, 'postcss.config.js'),
            },
         },
      },
   ]
};

var config = {
   mode: 'development',
   entry: {
      main: path.join(__dirname, 'src/boot.js')
   },
   output: {
      path: path.join(__dirname, '../api/', 'wwwroot'),
      filename: '[name].js',
      publicPath: '/'
   },
   module: {
      rules: [
         {
            test: /\.(js|jsx)$/,
            include: paths.appSrc,
            loader: require.resolve('babel-loader'),
            options: {
               presets: [
                  [
                     require.resolve('babel-preset-react-app'),
                     {
                        runtime: hasJsxRuntime ? 'automatic' : 'classic',
                     },
                  ],
               ],
            }
         },
         CSSLoader
      ]
   },
   plugins: [      
      new webpack.ProgressPlugin(),
      new MiniCssExtractPlugin({ filename: '[name].css' }),
   ]
}

module.exports = config;