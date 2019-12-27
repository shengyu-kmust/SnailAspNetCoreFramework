
<template>
  <!-- eslint-disable vue/require-component-is -->
  <!--component标签的用法参考 https://cn.vuejs.org/v2/guide/components.html#%E5%8A%A8%E6%80%81%E7%BB%84%E4%BB%B6 -->
  <!-- 下面的含义：动态决定组件是什么，如果to传入的是一个外部url，则解析成a标签，否则解析成router-link组件 -->
  <component v-bind="linkProps(to)">
    <slot />
  </component>
</template>

<script>
import { isExternal } from '@/utils/validate'

export default {
  props: {
    to: {
      type: String,
      required: true
    }
  },
  methods: {
    linkProps(url) {
      if (isExternal(url)) {
        //is为要动态解析成的组件名
        return {
          is: 'a',
          href: url,
          target: '_blank',
          rel: 'noopener'
        }
      }
      return {
        is: 'router-link',
        to: url
      }
    }
  }
}
</script>
