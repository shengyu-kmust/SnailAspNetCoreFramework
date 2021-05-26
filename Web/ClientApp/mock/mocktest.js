const Mock = require('mockjs')
const successCode = 2000
module.exports = [
  {
    url: '/api/mock/list',
    type: 'get',
    response: config => {
      var tmpData = Mock.mock({
        'array|20': [
          {
            'name': '@cname',
            'age': '@integerr(10,30)'
          }
        ]
      }).array
      return {
        code: successCode,
        data: tmpData
      }
    }
  },
  {
    url: '/api/mock/post',
    type: 'post',
    response: config => {
      return {
        code: successCode,
        data: 'success'
      }
    }
  }
]
