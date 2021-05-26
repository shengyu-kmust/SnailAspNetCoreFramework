/**
 * 请不要修改，为框架通用配置，便于统一更新
 */
import Vue from 'vue'
import Cookies from 'js-cookie'
import 'normalize.css/normalize.css' // A modern alternative to CSS resets
import ElementUI from 'element-ui'
import './styles/element-variables.scss'
import '@/styles/index.scss' // global css
import locale from 'element-ui/lib/locale/lang/zh-CN' // lang i18n
import App from './App'
import store from './store' // vuex
import router from './router'// router
import './icons' // icon
import './permission' // permission control
import './utils/error-log' // error log
import * as filters from './filters' // global filters
import setVm from '@/utils/setVm'
import permission from '@/directive/permission'
import '@/utils/registerComponent'// 下面开始注册全局组件

setVm(Vue) // 将常用的挂载到vue对象
permission.install(Vue) // 权限指令

// 如果要用mock本地，打开下面的，项目的其它配置不要变。本项目mock和两种，一个是mock server，一个是下面的mock本地（即MOCK XHR）
// 如果用mock本地，在chrome的network里是看不到请求的
// 如果要用mock server，要注释下面的，并用npm run dev:mock运行项目
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
