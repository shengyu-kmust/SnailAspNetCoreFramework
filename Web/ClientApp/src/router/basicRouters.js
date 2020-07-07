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
    {
      path: 'user',
      component: () => import('@/views/system/user'),
      name: 'user',
      meta: { title: '用户管理' }
    }
  ]
}

export default basicRouters
