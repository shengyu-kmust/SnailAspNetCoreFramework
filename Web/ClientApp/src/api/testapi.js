import request from '@/utils/request'
export const getMockList = params => request.get('/api/mock/list', { params })
export const postMock = data => request.post('/api/mock/post', data)

