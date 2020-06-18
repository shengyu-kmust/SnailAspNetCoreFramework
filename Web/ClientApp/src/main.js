import Vue from 'vue'

import Cookies from 'js-cookie'
import 'normalize.css/normalize.css' // A modern alternative to CSS resets

import ElementUI from 'element-ui'
import './styles/element-variables.scss'
import '@/styles/index.scss' // global css

import locale from 'element-ui/lib/locale/lang/zh-CN' // lang i18n

import App from './App'
import store from './store'
import router from './router'
import setVm from '@/utils/setVm'
setVm(Vue)

import './icons' // icon
import './permission' // permission control
import './utils/error-log' // error log
import * as filters from './filters' // global filters

// 下面开始注册全局组件
import '@/utils/registerComponent'
/**
 * If you don't want to use mock-server
 * you want to use MockJs for mock api
 * you can execute: mockXHR()
 *
 * Currently MockJs will be used in the production environment,
 * please remove it before going online! ! !
 */
// 如果要用mock本地，打开下面的，项目的其它配置不要变。本项目mock和两种，一个是mock server，一个是下面的mock本地（即MOCK XHR）
// 如果用mock本地，在chrome的network里是看不到请求的
// 如果要用mock server，要注释下面的，并要在vue.config.js里的devServer的proxy和after进行配置
// import { mockXHR } from '../mock'
// if (process.env.NODE_ENV === 'development') {
//   console.log('-------------------mockXHR-----------------')
//   console.log(JSON.stringify(process.env))
//   mockXHR()
// }

// set ElementUI lang to EN
Vue.use(ElementUI, {
  locale,
  size: Cookies.get('size') || 'medium' // set element-ui default size
})

// register global utility filters
Object.keys(filters).forEach(key => {
  Vue.filter(key, filters[key])
})

// 如果想要中文版 element-ui，按如下方式声明
// Vue.use(ElementUI)

Vue.config.productionTip = false

new Vue({
  el: '#app',
  router,
  store,
  render: h => h(App)
})
