// Auto-generated from TestConstants.cs

import TestEnum from './TestEnum';

const TestConstants = {
  appName: 'MyApp',
  nullValue: null,
  quoted: 'It\'s "quoted"\n',
  maxPageSize: 100,
  longValue: 9007199254740993,
  pi: 3.14159,
  floatValue: 1.5,
  price: 19.99,
  isEnabled: true,
  separator: ';',
  defaultEnum: TestEnum.Value2,
  unregisteredEnum: 1,
  undefinedEnumValue: 99,
} as const;

export default TestConstants;
