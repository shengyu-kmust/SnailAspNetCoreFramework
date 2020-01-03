// 将常用的东西挂载到vue实例下，这样在vue里就能用this.$api这种方法快捷访问
// import dayjs from 'dayjs'
// import _ from 'lodash'
import * as api from '@/api'
export default (Vue) => {
//   Vue.prototype.$dayjs = dayjs
  Vue.prototype.$api = api
}
