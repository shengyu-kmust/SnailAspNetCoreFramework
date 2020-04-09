import request from '@/utils/request'

export const login = data => request.post('/api/permission/login', data)
export const getInfo = token => request.get('/api/permission/getUserInfo', { params: { token: token }})
export const getAllResourceRoles = () => request.get('/api/permission/getAllResourceRoles')
// export function getInfo(token) {
//   return request({
//     url: '/user/info',
//     method: 'get',
//     params: { token }
//   })
// }

export function logout() {
  return request({
    url: '/user/logout',
    method: 'post'
  })
}
