import Layout from '@/layout'
const basicRouters = {
  path: '/basic',
  component: Layout,
  redirect: 'noRedirect',
  name: 'basic',
  meta: {
    title: '基础信息管理',
    iconClass: 'el-icon-setting'
  },
  children: [
   
  ]
}

export default basicRouters
