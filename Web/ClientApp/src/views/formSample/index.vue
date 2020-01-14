<template>
  <div>
    <snail-form :fields="fields" :data="data" />
    <button @click="getData" />
  </div>
</template>

<script>
import SnailForm from '@/components/Form/SnailForm'

export default {
  components: {
    SnailForm
  },
  data() {
    return {
      data: {
        str: 'this is str',
        date: '2019-12-12',
        select: 'yes'
      },
      fields: [
        {
          name: 'str',
          type: 'input',
          span: 12,
          label: '字段类型'
        },
        {
          name: 'date',
          type: 'date',
          span: 12,
          label: '时间'
        },
        {
          name: 'select',
          type: 'select',
          span: 8,
          label: '时间'
        }
      ]
    }
  },
  methods: {
    getData() {

    }
  }
}
</script>
