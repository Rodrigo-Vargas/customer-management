const path = require('path');

const paths = require('./paths');

const hasJsxRuntime = (() => { 
   try {
     require.resolve('react/jsx-runtime');
     return true;
   } catch (e) {
     return false;
   }
 })();

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
         }
      ]
   }
}

module.exports = config;